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

    /// <inheritdoc/>
    public int ResponseCode { get; private set; }

    /// <inheritdoc/>
    public string ResponseStatus { get; private set; } = "Unknown";

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
            .ContainsKey(name))
        {
            HttpClient
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
            HttpClient,
            _enc);

        if (options.Value
            .Retries
            .ContainsKey(name))
        {
            _retryPolicy = new(
                HttpClient,
                _enc,
                options
                    .Value
                    .Retries[name]);
        }
    }

    /// <inheritdoc/>
    public async Task<TResult?> GetAsync<TResult>(
        string route,
        HttpQuery? query = default,
        AuthenticationHeaderValue? authHeader = default,
        IEnumerable<RequestHeader>? additionalHeaders = default,
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
            cancellationToken);

    /// <inheritdoc/>
    public async Task GetAsync(
        string route,
        HttpQuery? query = default,
        AuthenticationHeaderValue? authHeader = default,
        IEnumerable<RequestHeader>? additionalHeaders = default,
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
            cancellationToken);

    /// <inheritdoc/>
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
            cancellationToken);

    /// <inheritdoc/>
    public async Task PutAsync(
        string route,
        object body,
        AuthenticationHeaderValue? authHeader = default,
        IEnumerable<RequestHeader>? additionalHeaders = default,
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
            cancellationToken);

    /// <inheritdoc/>
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
            cancellationToken);

    /// <inheritdoc/>
    public async Task PostAsync(
        string route,
        object body,
        AuthenticationHeaderValue? authHeader = default,
        IEnumerable<RequestHeader>? additionalHeaders = default,
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
            cancellationToken);

    /// <inheritdoc/>
    public async Task<TResult?> DeleteAsync<TResult>(
        string route,
        HttpQuery? query = default,
        AuthenticationHeaderValue? authHeader = default,
        IEnumerable<RequestHeader>? additionalHeaders = default,
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
            cancellationToken);

    /// <inheritdoc/>
    public async Task DeleteAsync(
        string route,
        HttpQuery? query = default,
        AuthenticationHeaderValue? authHeader = default,
        IEnumerable<RequestHeader>? additionalHeaders = default,
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
            cancellationToken);

    /// <inheritdoc/>
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
            cancellationToken);

    /// <inheritdoc/>
    public async Task PatchAsync(
        string route,
        object body,
        AuthenticationHeaderValue? authHeader = default,
        IEnumerable<RequestHeader>? additionalHeaders = default,
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
            cancellationToken);

    /// <inheritdoc/>
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
            cancellationToken);
    }

    /// <inheritdoc/>
    public async Task PostFormAsync(
        string route,
        IEnumerable<FormElement> elements,
        AuthenticationHeaderValue? authHeader = default,
        IEnumerable<RequestHeader>? additionalHeaders = default,
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
            cancellationToken);
    }

    /// <inheritdoc/>
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
            cancellationToken);
    }

    /// <inheritdoc/>
    public async Task PostUrlEncodedFormAsync(
        string route,
        IEnumerable<KeyValuePair<string, string?>> elements,
        AuthenticationHeaderValue? authHeader = default,
        IEnumerable<RequestHeader>? additionalHeaders = default,
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
