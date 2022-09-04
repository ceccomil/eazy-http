namespace EazyHttp;

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
                .GetAsync(url),
            route,
            query,
            body: null,
            authHeader,
            additionalHeaders,
            cancellationToken);

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

        using var response = await sendAsync(
            url,
            await GetContentFromBody(
                body,
                cancellationToken));

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

    private void AddHeaders(IEnumerable<RequestHeader>? headers)
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
}
