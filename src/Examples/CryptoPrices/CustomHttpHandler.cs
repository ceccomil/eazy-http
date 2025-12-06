using CaptainLogger.Generated;
using Microsoft.Extensions.Logging;
using System.Net;

namespace CryptoPrices;

public class CustomHttpHandler : HttpClientHandler
{
    private readonly ILogger _logger;

    public CustomHttpHandler(
        ILogger<CustomHttpHandler> logger)
        : base()
    {
        _logger = logger;

        AutomaticDecompression = DecompressionMethods.All;
    }

    protected override async Task<HttpResponseMessage> SendAsync(
        HttpRequestMessage request,
        CancellationToken cancellationToken)
    {
        _logger
            .InformationLog(
                request.Method,
                request.RequestUri);

        var result = await base
            .SendAsync(
                request,
                cancellationToken);

        _logger
            .InformationLog(
                (int)result.StatusCode,
                result.ReasonPhrase,
                result.Version,
                result
                    .Content
                    .GetType()
                    .Name);

        return result;
    }
}
