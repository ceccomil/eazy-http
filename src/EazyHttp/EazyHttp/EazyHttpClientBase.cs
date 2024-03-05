namespace EazyHttp;

/// <summary>
/// Base class for each EazyHttpClient implementation.
/// </summary>
public abstract partial class EazyHttpClientBase : IEazyHttpClient
{
    private readonly Encoding _enc;

    private readonly JsonSerializerOptions _serializer;

    /// <inheritdoc/>
    public HttpClient HttpClient { get; }

    /// <summary>
    /// Latest response results.
    /// </summary>
    public ResponseResultsCollection ResponseResults { get; } = [];

    /// <inheritdoc/>
    public int ResponseCode => ResponseResults
        .Last?
        .ResponseCode
        ?? 0;

    /// <inheritdoc/>
    public string ResponseStatus => ResponseResults
        .Last?
        .ResponseStatus
        ?? "Unknown";

    /// <inheritdoc/>
    public MediaTypeHeaderValue? ResponseContentType => ResponseResults
        .Last?
        .ResponseContentType;

    /// <inheritdoc/>
    public string ResponseContentTypeDescription => ResponseResults
        .Last?
        .ResponseContentTypeDescription
        ?? "Unknown";

    /// <inheritdoc/>
    public ContentDispositionHeaderValue? ResponseContentDisposition => ResponseResults
        .Last?
        .ResponseContentDisposition;

    /// <summary>
    /// The collection of headers included in the response.
    /// </summary>
    public HttpContentHeaders? ResponseHeaders => ResponseResults
        .Last?
        .ResponseHeaders;

    /// <inheritdoc/>
    public HttpRequestHeaders Headers => HttpClient
        .DefaultRequestHeaders;

    /// <summary>
    /// Base constructor
    /// </summary>
    public EazyHttpClientBase(
        HttpClient httpClient,
        IOptions<EazyClientOptions> options)
    {
        HttpClient = httpClient;

        var name = GetType()
            .Name;

        if (options.Value
            .PersistentHeaders
            .TryGetValue(name,
            out var headValue))
        {
            HttpClient
            .AddHeaders(
                headValue);
        }

        _enc = Encoding.UTF8;

        if (options.Value
            .Encodings
            .TryGetValue(name,
            out var encValue))
        {
            _enc = encValue;
        }

        _serializer = new(
            JsonSerializerDefaults
            .Web);

        if (options.Value
            .SerializersOptions
            .TryGetValue(name,
            out var serValue))
        {
            _serializer = serValue;
        }

        _retryPolicy = new(
            HttpClient,
            _enc);

        if (options.Value
            .Retries
            .TryGetValue(name,
            out var retValue))
        {
            _retryPolicy = new(
                HttpClient,
                _enc,
                retValue);
        }
    }

    /// <inheritdoc/>
    public async Task<TResult?> GetAsync<TResult>(
        string route,
        HttpQuery? query = default,
        AuthenticationHeaderValue? authHeader = default,
        IEnumerable<RequestHeader>? additionalHeaders = default,
        Guid? requestId = default,
        CancellationToken cancellationToken = default)
        where TResult : class => await HttpAsync<TResult>(
            async (url, content) => await HttpClient
                .GetAsync(url, cancellationToken),
            HttpMethod.Get,
            route,
            query,
            body: null,
            authHeader,
            additionalHeaders,
            requestId,
            cancellationToken);

    /// <inheritdoc/>
    public async Task GetAsync(
        string route,
        HttpQuery? query = default,
        AuthenticationHeaderValue? authHeader = default,
        IEnumerable<RequestHeader>? additionalHeaders = default,
        Guid? requestId = default,
        CancellationToken cancellationToken = default) =>
        await HttpAsync(
            async (url, content) => await HttpClient
                .GetAsync(url, cancellationToken),
            HttpMethod.Get,
            route,
            query,
            body: null,
            authHeader,
            additionalHeaders,
            requestId,
            cancellationToken);

    /// <inheritdoc/>
    public async Task<TResult?> PutAsync<TResult>(
        string route,
        object body,
        AuthenticationHeaderValue? authHeader = default,
        IEnumerable<RequestHeader>? additionalHeaders = default,
        Guid? requestId = default,
        CancellationToken cancellationToken = default)
        where TResult : class => await HttpAsync<TResult>(
            async (url, content) =>
            {
                SetApplicationJson(content);

                return await HttpClient
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
            requestId,
            cancellationToken);

    /// <inheritdoc/>
    public async Task PutAsync(
        string route,
        object body,
        AuthenticationHeaderValue? authHeader = default,
        IEnumerable<RequestHeader>? additionalHeaders = default,
        Guid? requestId = default,
        CancellationToken cancellationToken = default) =>
        await HttpAsync(
            async (url, content) =>
            {
                SetApplicationJson(content);

                return await HttpClient
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
            requestId,
            cancellationToken);

    /// <inheritdoc/>
    public async Task<TResult?> PostAsync<TResult>(
        string route,
        object body,
        AuthenticationHeaderValue? authHeader = default,
        IEnumerable<RequestHeader>? additionalHeaders = default,
        Guid? requestId = default,
        CancellationToken cancellationToken = default)
        where TResult : class => await HttpAsync<TResult>(
            async (url, content) =>
            {
                SetApplicationJson(content);

                return await HttpClient
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
            requestId,
            cancellationToken);

    /// <inheritdoc/>
    public async Task PostAsync(
        string route,
        object body,
        AuthenticationHeaderValue? authHeader = default,
        IEnumerable<RequestHeader>? additionalHeaders = default,
        Guid? requestId = default,
        CancellationToken cancellationToken = default) =>
        await HttpAsync(
            async (url, content) =>
            {
                SetApplicationJson(content);

                return await HttpClient
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
            requestId,
            cancellationToken);

    /// <inheritdoc/>
    public async Task<TResult?> DeleteAsync<TResult>(
        string route,
        HttpQuery? query = default,
        AuthenticationHeaderValue? authHeader = default,
        IEnumerable<RequestHeader>? additionalHeaders = default,
        Guid? requestId = default,
        CancellationToken cancellationToken = default)
        where TResult : class => await HttpAsync<TResult>(
            async (url, content) => await HttpClient
                .DeleteAsync(url, cancellationToken),
            HttpMethod.Delete,
            route,
            query,
            body: null,
            authHeader,
            additionalHeaders,
            requestId,
            cancellationToken);

    /// <inheritdoc/>
    public async Task DeleteAsync(
        string route,
        HttpQuery? query = default,
        AuthenticationHeaderValue? authHeader = default,
        IEnumerable<RequestHeader>? additionalHeaders = default,
        Guid? requestId = default,
        CancellationToken cancellationToken = default) =>
        await HttpAsync(
            async (url, content) => await HttpClient
                .DeleteAsync(url, cancellationToken),
            HttpMethod.Delete,
            route,
            query,
            body: null,
            authHeader,
            additionalHeaders,
            requestId,
            cancellationToken);

    /// <inheritdoc/>
    public async Task<TResult?> PatchAsync<TResult>(
        string route,
        object body,
        AuthenticationHeaderValue? authHeader = default,
        IEnumerable<RequestHeader>? additionalHeaders = default,
        Guid? requestId = default,
        CancellationToken cancellationToken = default)
        where TResult : class => await HttpAsync<TResult>(
            async (url, content) =>
            {
                SetApplicationJson(content);

                return await HttpClient
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
            requestId,
            cancellationToken);

    /// <inheritdoc/>
    public async Task PatchAsync(
        string route,
        object body,
        AuthenticationHeaderValue? authHeader = default,
        IEnumerable<RequestHeader>? additionalHeaders = default,
        Guid? requestId = default,
        CancellationToken cancellationToken = default) =>
        await HttpAsync(
            async (url, content) =>
            {
                SetApplicationJson(content);

                return await HttpClient
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
            requestId,
            cancellationToken);

    /// <inheritdoc/>
    public async Task<TResult?> PostFormAsync<TResult>(
        string route,
        IEnumerable<FormElement> elements,
        AuthenticationHeaderValue? authHeader = default,
        IEnumerable<RequestHeader>? additionalHeaders = default,
        Guid? requestId = default,
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
            async (url, content) => await HttpClient
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
            requestId,
            cancellationToken);
    }

    /// <inheritdoc/>
    public async Task PostFormAsync(
        string route,
        IEnumerable<FormElement> elements,
        AuthenticationHeaderValue? authHeader = default,
        IEnumerable<RequestHeader>? additionalHeaders = default,
        Guid? requestId = default,
        CancellationToken cancellationToken = default)
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

        await HttpAsync(
            async (url, content) => await HttpClient
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
            requestId,
            cancellationToken);
    }

    /// <inheritdoc/>
    public async Task<TResult?> PostUrlEncodedFormAsync<TResult>(
        string route,
        IEnumerable<KeyValuePair<string, string?>> elements,
        AuthenticationHeaderValue? authHeader = default,
        IEnumerable<RequestHeader>? additionalHeaders = default,
        Guid? requestId = default,
        CancellationToken cancellationToken = default)
        where TResult : class
    {
        var form = new FormUrlEncodedContent(
            elements);

        return await HttpAsync<TResult>(
            async (url, content) => await HttpClient
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
            requestId,
            cancellationToken);
    }

    /// <inheritdoc/>
    public async Task PostUrlEncodedFormAsync(
        string route,
        IEnumerable<KeyValuePair<string, string?>> elements,
        AuthenticationHeaderValue? authHeader = default,
        IEnumerable<RequestHeader>? additionalHeaders = default,
        Guid? requestId = default,
        CancellationToken cancellationToken = default)
    {
        var form = new FormUrlEncodedContent(
            elements);

        await HttpAsync(
            async (url, content) => await HttpClient
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
            requestId,
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
