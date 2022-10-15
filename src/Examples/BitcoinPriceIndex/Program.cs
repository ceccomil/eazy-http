using Microsoft.Extensions.DependencyInjection;
using EazyHttp;
using EazyHttp.HttpClients;
using CaptainLogger;
using CaptainLogger.Options;
using Microsoft.Extensions.Logging;
using EazyHttp.Contracts;
using BitcoinPriceIndex;

var services = new ServiceCollection()
    .ConfigureEazyHttpClients(opts =>
    {
        opts
            .EazyHttpClients
            .Add(new(
                "MessariClient",
                "https://data.messari.io/api/v2"));
    })
    .AddEazyHttpClients()
    .Configure<CaptainLoggerOptions>(opts =>
    {
        opts.TimeIsUtc = true;
        opts.LogRecipients = Recipients.Console;
        opts.ArgumentsCount = LogArguments.Three;
        opts.Templates.Add(
                LogArguments.Three,
                "Today's {Position} coin is:\r\n{Slug} at {Price:N10} USD");

    })
    .AddLogging(builder =>
    {
        builder
            .ClearProviders()
            .AddCaptainLogger()
            .AddFilter(
                "",
                LogLevel.Information);
    });


using var sp = services
    .BuildServiceProvider();

using var scope = sp
    .CreateScope();

var logger = sp
    .GetRequiredService<ICaptainLogger<MessariClient>>();

logger
    .InformationLog("Application is starting");

logger
    .InformationLog("Getting Eazy Http clients from DI container");

var coinClient = sp
    .GetRequiredService<IEazyClients>()
    .MessariClient;

var query = new HttpQuery();

logger
    .InformationLog(
        "Setting up query parameters");

query.AddParam(new("fields", "id,slug,symbol,metrics/market_data/price_usd"));

logger
    .InformationLog("Sending request");

var result = await coinClient
    .GetAsync<ResultDto>(
        "assets",
        query)
    ?? throw new NullReferenceException();

var max = result
    .Data
    .MaxBy(x => x.Metrics.MarketData.PriceUsd)
    ?? throw new NullReferenceException();

logger
    .InformationLog(
        "Max valued",
        max.Slug,
        max.Metrics.MarketData.PriceUsd);

var min = result
    .Data
    .MinBy(x => x.Metrics.MarketData.PriceUsd)
    ?? throw new NullReferenceException();

logger
    .InformationLog(
        "Min valued",
        min.Slug,
        min.Metrics.MarketData.PriceUsd);
