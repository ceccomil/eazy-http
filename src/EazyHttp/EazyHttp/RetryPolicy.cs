namespace EazyHttp;

internal class RetryPolicy
{
    private readonly Dictionary<Guid, Jitter> _jitters = new();
    private readonly RetryConfiguration _conf;
    private readonly Random _random = new();
    private readonly HttpClient _httpClient;
    private readonly Encoding _enc;

    private const int DEFAULT_GAP = 5;

    public RetryPolicy(
        HttpClient httpClient,
        Encoding enc,
        RetryConfiguration? configuration = default)
    {
        _conf = configuration
            ?? new();

        if (_conf.Seed.HasValue)
        {
            _random = new Random(
                _conf.Seed.Value);
        }

        _enc = enc;

        _httpClient = httpClient;
    }

    public async Task<ResponseContent> SendAndRetry(
        Func<string, HttpContent?, Task<HttpResponseMessage>> sendAsync,
        HttpMethod httpMethod,
        string url,
        HttpContent? content,
        AuthenticationHeaderValue? authHeader = default,
        IEnumerable<RequestHeader>? additionalHeaders = default,
        CancellationToken cancellationToken = default)
    {
        var attempts = 0;
        var reqId = Guid.NewGuid();

        _jitters.Add(
            reqId,
            new());

    HttpSend:
        attempts++;

        _httpClient
            .AddHeaders(additionalHeaders);

        if (authHeader is not null)
        {
            _httpClient
                .DefaultRequestHeaders
                .Authorization = authHeader;
        }

        var response = await sendAsync(
            url,
            content);

        if (authHeader is not null)
        {
            _httpClient
                .DefaultRequestHeaders
                .Authorization = null;
        }

        _httpClient
            .RemoveHeaders(additionalHeaders);

        var rc = new ResponseContent(
            response,
            await response
                .Content
                .ReadAsStreamAsync(
                    cancellationToken));

        if (!rc.IsSuccess)
        {
            await HandleFailure(
                rc,
                httpMethod,
                attempts,
                url,
                reqId,
                cancellationToken);

            goto HttpSend;
        }

        return rc;
    }

    private async Task HandleFailure(
        ResponseContent rc,
        HttpMethod httpMethod,
        int attempts,
        string url,
        Guid reqId,
        CancellationToken cancellationToken)
    {
        var buffer = new byte[rc.Content.Length];
        rc.Content.Read(buffer, 0, buffer.Length);

        var exceptions = _jitters[reqId]
            .FailedRequests;

        var respContent = _enc
            .GetString(buffer);

        exceptions
            .Add(
                new FailedRequestException(
                    rc.StatusCode,
                    httpMethod,
                    $"{DateTime.UtcNow} Request did not succeed ({url}" +
                    $") [{httpMethod} {(int)rc.StatusCode}]" +
                    $" {rc.StatusCode}, result:" +
                    $"\r\n{respContent}",
                    respContent));

        if (attempts >= _conf.MaxAttempts ||
            !_conf.StatusCodeMatchingCondition(
                rc.StatusCode,
                httpMethod))
        {
            Exception ex = exceptions
                .First();

            if (exceptions.Count > 1)
            {
                ex = new AggregateException(
                "All the available retries have been completed unsuccessfully",
                exceptions);
            }

            _jitters
                .Remove(reqId);

            throw ex;
        }

        var rng = (_random
            .Next(100, 500) / 1000.0d) +
            1.0d;

        var ts = TimeSpan
            .FromSeconds(
                (2.0 * _jitters[reqId].LastWait.TotalSeconds) +
                (DEFAULT_GAP * rng));

        _jitters[reqId]
            .LastWait = ts;

        await Task
            .Delay(ts,
            cancellationToken);
    }
}
