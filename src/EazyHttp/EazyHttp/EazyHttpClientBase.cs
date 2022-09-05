﻿namespace EazyHttp;

/// <summary>
/// TODO documentation
/// </summary>
public abstract class EazyHttpClientBase : IEazyHttpClient
{
    private readonly HttpClient _httpClient;
    private readonly EazyClientOptions _options;

    private readonly Encoding _enc = Encoding.UTF8;

    /// <summary>
    /// Gets the status code of the last HTTP request 
    /// </summary>
    public int ResponseCode { get; private set; }

    /// <summary>
    /// Gets the status description of the last HTTP request 
    /// </summary>
    public string ResponseStatus { get; private set; } = "Unknown";

    /// <summary>
    /// Http request headers
    /// </summary>
    public HttpRequestHeaders Headers
    {
        get => _httpClient
            .DefaultRequestHeaders;
    }

    /// <summary>
    /// TODO docume
    /// </summary>
    /// <param name="httpClient"></param>
    /// <param name="options"></param>
    public EazyHttpClientBase(
        HttpClient httpClient,
        IOptions<EazyClientOptions> options)
    {
        _httpClient = httpClient;
        _options = options.Value;

        var name = GetType()
            .Name;

        if (_options
            .PersistentHeaders
            .ContainsKey(name))
        {
            AddHeaders(
                _options
                .PersistentHeaders[name]);
        }
    }

    /// <summary>
    /// TODO documentation
    /// </summary>
    public async Task<TResult?> GetAsync<TResult>(
        string route,
        HttpQuery? query = default,
        AuthenticationHeaderValue? authHeader = default,
        IEnumerable<RequestHeader>? additionalHeaders = default,
        CancellationToken cancellationToken = default)
        where TResult : class => await HttpAsync<TResult>(
            async (url, content) => await _httpClient
                .GetAsync(url, cancellationToken),
            route,
            query,
            body: null,
            authHeader,
            additionalHeaders,
            cancellationToken);

    /// <summary>
    /// TODO docume
    /// </summary>
    /// <typeparam name="TResult"></typeparam>
    /// <param name="route"></param>
    /// <param name="body"></param>
    /// <param name="authHeader"></param>
    /// <param name="additionalHeaders"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public async Task<TResult?> PutAsync<TResult>(
        string route,
        object body,
        AuthenticationHeaderValue? authHeader = default,
        IEnumerable<RequestHeader>? additionalHeaders = default,
        CancellationToken cancellationToken = default)
        where TResult : class => await HttpAsync<TResult>(
            async (url, content) =>
            {
                SetApplicationJson(content);

                return await _httpClient
                    .PutAsync(
                        url,
                        content,
                        cancellationToken);
            },
            route,
            query: null,
            body,
            authHeader,
            additionalHeaders,
            cancellationToken);

    /// <summary>
    /// TODO Docume
    /// </summary>
    /// <typeparam name="TResult"></typeparam>
    /// <param name="route"></param>
    /// <param name="body"></param>
    /// <param name="authHeader"></param>
    /// <param name="additionalHeaders"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public async Task<TResult?> PostAsync<TResult>(
        string route,
        object body,
        AuthenticationHeaderValue? authHeader = default,
        IEnumerable<RequestHeader>? additionalHeaders = default,
        CancellationToken cancellationToken = default)
        where TResult : class => await HttpAsync<TResult>(
            async (url, content) =>
            {
                SetApplicationJson(content);

                return await _httpClient
                    .PostAsync(
                        url,
                        content,
                        cancellationToken);
            },
            route,
            query: null,
            body,
            authHeader,
            additionalHeaders,
            cancellationToken);

    /// <summary>
    /// TODO Document
    /// </summary>
    /// <typeparam name="TResult"></typeparam>
    /// <param name="route"></param>
    /// <param name="query"></param>
    /// <param name="authHeader"></param>
    /// <param name="additionalHeaders"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public async Task<TResult?> DeleteAsync<TResult>(
        string route,
        HttpQuery? query = default,
        AuthenticationHeaderValue? authHeader = default,
        IEnumerable<RequestHeader>? additionalHeaders = default,
        CancellationToken cancellationToken = default)
        where TResult : class => await HttpAsync<TResult>(
            async (url, content) => await _httpClient
                .DeleteAsync(url, cancellationToken),
            route,
            query,
            body: null,
            authHeader,
            additionalHeaders,
            cancellationToken);

    /// <summary>
    /// TODO docume
    /// </summary>
    /// <typeparam name="TResult"></typeparam>
    /// <param name="route"></param>
    /// <param name="body"></param>
    /// <param name="authHeader"></param>
    /// <param name="additionalHeaders"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public async Task<TResult?> PatchAsync<TResult>(
        string route,
        object body,
        AuthenticationHeaderValue? authHeader = default,
        IEnumerable<RequestHeader>? additionalHeaders = default,
        CancellationToken cancellationToken = default)
        where TResult : class => await HttpAsync<TResult>(
            async (url, content) =>
            {
                SetApplicationJson(content);

                return await _httpClient
                    .PatchAsync(
                        url,
                        content,
                        cancellationToken);
            },
            route,
            query: null,
            body,
            authHeader,
            additionalHeaders,
            cancellationToken);

    /// <summary>
    /// TODO docume
    /// </summary>
    /// <typeparam name="TResult"></typeparam>
    /// <param name="route"></param>
    /// <param name="elements"></param>
    /// <param name="authHeader"></param>
    /// <param name="additionalHeaders"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public async Task<TResult?> PostFormAsync<TResult>(
        string route,
        IEnumerable<FormElement> elements,
        AuthenticationHeaderValue? authHeader = default,
        IEnumerable<RequestHeader>? additionalHeaders = default,
        CancellationToken cancellationToken = default)
        where TResult : class
    {
        var form = new MultipartFormDataContent();

        foreach (var element in elements)
        {
            if (element.SendAsString ||
                element.FileName is null)
            {
                form.Add(
                    element.HttpElementContent,
                    element.QueryParam);
            }
            else
            {
                form.Add(
                    element.HttpElementContent,
                    element.QueryParam,
                    element.FileName);
            }
        }

        return await HttpAsync<TResult>(
            async (url, content) => await _httpClient
                .SendAsync(
                    new(HttpMethod.Post, url)
                    {
                        Content = form
                    },
                    cancellationToken),
            route,
            query: null,
            body: null,
            authHeader,
            additionalHeaders,
            cancellationToken);
    }

    /// <summary>
    /// TODO docume
    /// </summary>
    /// <typeparam name="TResult"></typeparam>
    /// <param name="route"></param>
    /// <param name="elements"></param>
    /// <param name="authHeader"></param>
    /// <param name="additionalHeaders"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public async Task<TResult?> PostUrlEncodedFormAsync<TResult>(
        string route,
        IEnumerable<KeyValuePair<string, string?>> elements,
        AuthenticationHeaderValue? authHeader = default,
        IEnumerable<RequestHeader>? additionalHeaders = default,
        CancellationToken cancellationToken = default)
        where TResult : class
    {
        var form = new FormUrlEncodedContent(
            elements);

        return await HttpAsync<TResult>(
            async (url, content) => await _httpClient
                .SendAsync(
                    new(HttpMethod.Post, url)
                    {
                        Content = form
                    },
                    cancellationToken),
            route,
            query: null,
            body: null,
            authHeader,
            additionalHeaders,
            cancellationToken);
    }


    private async Task<TResult?> HttpAsync<TResult>(
        Func<string, HttpContent?, Task<HttpResponseMessage>> sendAsync,
        string route,
        HttpQuery? query = default,
        object? body = default,
        AuthenticationHeaderValue? authHeader = default,
        IEnumerable<RequestHeader>? additionalHeaders = default,
        CancellationToken cancellationToken = default) where TResult : class
    {
        var url = CombineUrl(route, query);
        if (authHeader is not null)
        {
            _httpClient
                .DefaultRequestHeaders
                .Authorization = authHeader;
        }

        AddHeaders(additionalHeaders);

        var content = await GetContentFromBody(
                body,
                cancellationToken);

        using var response = await sendAsync(
            url,
            content);

        RemoveHeaders(additionalHeaders);

        if (authHeader is not null)
        {
            _httpClient
                .DefaultRequestHeaders
                .Authorization = null;
        }

        ResponseCode = (int)response.StatusCode;
        ResponseStatus = $"{response.StatusCode}";

        using var stream = await response
            .Content
            .ReadAsStreamAsync(
                cancellationToken);

        if (!response.IsSuccessStatusCode)
        {
            var buffer = new byte[stream.Length];
            stream.Read(buffer, 0, buffer.Length);

            throw new FailedRequestException(
                $"Request didn't succed ({url}" +
                $") [{ResponseCode}] {ResponseStatus}," +
                $" result: {_enc.GetString(buffer)}");
        }

        return await JsonSerializer
            .DeserializeAsync<TResult>(
                stream,
                _options.SerializerOptions,
                cancellationToken);
    }

    private async Task<HttpContent?> GetContentFromBody(
        object? body,
        CancellationToken cancellationToken = default)
    {
        if (body is null)
        {
            return default;
        }

        using var stream = new MemoryStream();
        await JsonSerializer
            .SerializeAsync(
                stream,
                body,
                body.GetType(),
                _options.SerializerOptions,
                cancellationToken);

        stream.Seek(
            0,
            SeekOrigin.Begin);

        return new StringContent(
            _enc.GetString(
                stream.GetBuffer()),
            _enc,
            "application/json");
    }

    private string CombineUrl(
        string route,
        HttpQuery? query = default)
    {
        var url = _httpClient
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

    private void AddHeaders(
        IEnumerable<RequestHeader>? headers)
    {
        if (headers is null)
        {
            return;
        }

        foreach (var h in headers.Distinct())
        {
            _httpClient
                .DefaultRequestHeaders
                .Add(
                    h.Key,
                    h.Value);
        }
    }

    private void RemoveHeaders(
        IEnumerable<RequestHeader>? headers)
    {
        if (headers is null)
        {
            return;
        }

        foreach (var h in headers.Distinct())
        {
            _httpClient
                .DefaultRequestHeaders
                .Remove(
                    h.Key);
        }
    }

    private static void SetApplicationJson(
        HttpContent? content)
    {
        if (content is not null)
        {
            content
                .Headers
                .ContentType = new("application/json");
        }
    }
}
