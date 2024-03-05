namespace EazyHttp;

public abstract partial class EazyHttpClientBase
{
    private readonly RetryPolicy _retryPolicy;

    private async Task<TResult?> HttpAsync<TResult>(
        Func<string, HttpContent?, Task<HttpResponseMessage>> sendAsync,
        HttpMethod httpMethod,
        string route,
        HttpQuery? query = default,
        object? body = default,
        AuthenticationHeaderValue? authHeader = default,
        IEnumerable<RequestHeader>? additionalHeaders = default,
        Guid? requestId = default,
        CancellationToken cancellationToken = default) where TResult : class
    {
        using var response = await HttpResponseAsync(
            sendAsync,
            httpMethod,
            route,
            query,
            body,
            authHeader,
            additionalHeaders,
            requestId,
            cancellationToken);

        return await DeserializeOrGetBytes<TResult>(
            response.ContentHeaders,
            response.Content,
            cancellationToken);
    }

    private async Task HttpAsync(
        Func<string, HttpContent?, Task<HttpResponseMessage>> sendAsync,
        HttpMethod httpMethod,
        string route,
        HttpQuery? query = default,
        object? body = default,
        AuthenticationHeaderValue? authHeader = default,
        IEnumerable<RequestHeader>? additionalHeaders = default,
        Guid? requestId = default,
        CancellationToken cancellationToken = default)
    {
        using var response = await HttpResponseAsync(
            sendAsync,
            httpMethod,
            route,
            query,
            body,
            authHeader,
            additionalHeaders,
            requestId,
            cancellationToken);
    }

    private async Task<ResponseContent> HttpResponseAsync(
        Func<string, HttpContent?, Task<HttpResponseMessage>> sendAsync,
        HttpMethod httpMethod,
        string route,
        HttpQuery? query = default,
        object? body = default,
        AuthenticationHeaderValue? authHeader = default,
        IEnumerable<RequestHeader>? additionalHeaders = default,
        Guid? requestId = default,
        CancellationToken cancellationToken = default)
    {
        var url = CombineUrl(route, query);

        var content = await _serializer
            .GetContentFromBody(
                body,
                _enc,
                cancellationToken);

        var result = await _retryPolicy
            .SendAndRetry(
                sendAsync,
                httpMethod,
                url,
                content,
                authHeader,
                additionalHeaders,
                cancellationToken);

        ResponseResults
            .Add(new(requestId)
            {
                ResponseCode = (int)result
                    .Content
                    .StatusCode,
                ResponseStatus = $"{result.Content.StatusCode}",
                ResponseContentType = result
                    .Content
                    .ContentHeaders
                    .ContentType,
                ResponseContentDisposition = result
                    .Content
                    .ContentHeaders
                    .ContentDisposition,
                ResponseContentHeaders = result
                    .Content
                    .ContentHeaders,
                ResponseHeaders = result
                    .Content
                    .Headers
            });

        if (result.Failure is not null)
        {
            throw result.Failure;
        }

        return result.Content;
    }

    private async Task<TResult?> DeserializeOrGetBytes<TResult>(
        HttpContentHeaders headers,
        Stream stream,
        CancellationToken cancellationToken)
        where TResult : class
    {
        if (headers.Contains("Content-Type") &&
            headers.GetValues("Content-Type")
            .Any(x => x.Contains("application/json")))
        {
            return await JsonSerializer
                .DeserializeAsync<TResult>(
                    stream,
                    _serializer,
                    cancellationToken);
        }

        try
        {
            if (typeof(TResult) == typeof(Stream))
            {
                return await GetStream<TResult>(
                    stream,
                    cancellationToken);
            }

            return await GetStringOrBytes<TResult>(
                stream,
                cancellationToken);
        }
        catch (Exception ex)
        {
            throw new ByteArrayExpectedException(
                "Response content type is not a valid JSON. " +
                $"A `{nameof(String)}` or a `{nameof(Byte)}[]`" +
                $" or a `{nameof(Stream)}` was expected! " +
                "Check the inner exception for details!",
                ex);
        }
    }

    private static async Task<TResult?> GetStream<TResult>(
        Stream stream,
        CancellationToken cancellationToken)
        where TResult : class
    {
        var ms = new MemoryStream();
        await stream.CopyToAsync(
            ms,
            cancellationToken);

        return ms as TResult;
    }

    private async Task<TResult?> GetStringOrBytes<TResult>(
        Stream stream,
        CancellationToken cancellationToken)
        where TResult : class
    {
        var buffer = new byte[stream.Length];

        await stream.ReadAsync(
            buffer,
            cancellationToken);

        if (typeof(TResult) == typeof(string))
        {
            return (TResult?)Convert
                .ChangeType(
                    _enc.GetString(buffer),
                    typeof(TResult));
        }

        return (TResult?)Convert
            .ChangeType(
                buffer,
                typeof(TResult));
    }

    private string CombineUrl(
        string route,
        HttpQuery? query = default)
    {
        var url = HttpClient
            .BaseAddress?
            .ToString()
            ?? string.Empty;

        while (url.EndsWith('/'))
        {
            url = url.Remove(url.Length - 1);
        }

        url += "/";
        route = $"{route}";

        if (route.StartsWith(
            "http",
            StringComparison.OrdinalIgnoreCase))
        {
            url = string.Empty;
        }

        while (route.StartsWith('/'))
        {
            route = route[1..];
        }

        url += $"{route}{query}";

        return url;
    }
}
