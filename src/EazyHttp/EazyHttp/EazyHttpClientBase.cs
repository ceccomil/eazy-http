namespace EazyHttp;

/// <summary>
/// TODO documentation
/// </summary>
public abstract partial class EazyHttpClientBase : IEazyHttpClient
{
    private readonly HttpClient _httpClient;

    private readonly Encoding _enc;

    private readonly JsonSerializerOptions _serializer;

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
    public HttpRequestHeaders Headers => _httpClient
            .DefaultRequestHeaders;

    /// <inheritdoc/>
    public EazyHttpClientBase(
        HttpClient httpClient,
        IOptions<EazyClientOptions> options)
    {
        _httpClient = httpClient;

        var name = GetType()
            .Name;

        if (options.Value
            .PersistentHeaders
            .ContainsKey(name))
        {
            _httpClient
                .AddHeaders(
                    options.Value
                    .PersistentHeaders[name]);
        }

        _enc = Encoding.UTF8;

        if (options.Value
            .Encodings
            .ContainsKey(name))
        {
            _enc = options
                .Value
                .Encodings[name];
        }

        _serializer = new(
            JsonSerializerDefaults
            .Web);

        if (options.Value
            .SerializersOptions
            .ContainsKey(name))
        {
            _serializer = options
                .Value
                .SerializersOptions[name];
        }

        _retryPolicy = new(
            _httpClient,
            _enc);

        if (options.Value
            .Retries
            .ContainsKey(name))
        {
            _retryPolicy = new(
                _httpClient,
                _enc,
                options
                    .Value
                    .Retries[name]);
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
            HttpMethod.Get,
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
            HttpMethod.Put,
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
            HttpMethod.Post,
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
            HttpMethod.Delete,
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
            HttpMethod.Patch,
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
            HttpMethod.Post,
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
            HttpMethod.Post,
            route,
            query: null,
            body: null,
            authHeader,
            additionalHeaders,
            cancellationToken);
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
