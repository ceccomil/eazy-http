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
            cancellationToken);

        return await DeserializeOrGetBytes<TResult>(
            response.Headers,
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
        CancellationToken cancellationToken = default)
    {
        var url = CombineUrl(route, query);

        var content = await _serializer
            .GetContentFromBody(
                body,
                _enc,
                cancellationToken);

        var response = await _retryPolicy
            .SendAndRetry(
                sendAsync,
                httpMethod,
                url,
                content,
                authHeader,
                additionalHeaders,
                cancellationToken);

        ResponseCode = (int)response.StatusCode;
        ResponseStatus = $"{response.StatusCode}";

        return response;
    }

    private async Task<TResult?> DeserializeOrGetBytes<TResult>(
        HttpContentHeaders headers,
        Stream stream,
        CancellationToken cancellationToken)
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
            var buffer = new byte[stream.Length];
            await stream
                .ReadAsync(
                    buffer,
                    cancellationToken);

            if (typeof(TResult) == typeof(string))
            {
                return (TResult?)Convert
                .ChangeType(
                    _enc.GetString(buffer),
                    typeof(TResult?));
            }

            return (TResult?)Convert
                .ChangeType(
                    buffer,
                    typeof(TResult?));
        }
        catch (Exception ex)
        {
            throw new ByteArrayExpectedException(
                "Response content type is not a valid " +
                "JSON. A byte array TResult was expected!",
                ex);
        }
    }

    private string CombineUrl(
        string route,
        HttpQuery? query = default)
    {
        var url = HttpClient
            .BaseAddress?
            .ToString()
            ?? string.Empty;

        while (url.EndsWith("/"))
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

        while (route.StartsWith("/"))
        {
            route = route[1..];
        }

        url += $"{route}{query}";

        return url;
    }
}
