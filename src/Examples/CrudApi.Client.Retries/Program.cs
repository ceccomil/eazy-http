using Microsoft.Extensions.DependencyInjection;
using EazyHttp;
using System.Net;

// This example requires the WEB API (CrudAPI) to be started

var services = new ServiceCollection()
    .ConfigureEazyHttpClients(
        opts =>
        {
            opts
                .EazyHttpClients
                .Add(new(
                    "CrudApiClient",
                    "https://localhost:7240/"));

            opts
                .Retries
                .Add(
                    "CrudApiClient",
                    new()
                    {
                        MaxAttempts = 3,
                        StatusCodeMatchingCondition = (status, _) =>
                        {
                            if (status is HttpStatusCode.ServiceUnavailable
                                or HttpStatusCode.GatewayTimeout
                                or HttpStatusCode.TooManyRequests)
                            {
                                return true;
                            }

                            return false;
                        }
                    });
        })
    .AddEazyHttpClients();

using var sp = services
    .BuildServiceProvider();

using var scope = sp
    .CreateScope();

var clients = scope
    .ServiceProvider
    .GetRequiredService<IEazyClients>();

Console
    .WriteLine(
        "The API only returns succes in " +
        "the first 10 secs of any minute");

Console
    .WriteLine(
        $"Failures:{Environment.NewLine}" +
        $"10 <= 15 returns 400{Environment.NewLine}" +
        $"15 <= 30 returns 503{Environment.NewLine}" +
        $"30 <= 45 returns 504{Environment.NewLine}" +
        $"> 45 returns 429{Environment.NewLine}");

Console
    .WriteLine(
        "Retry policy: 3 retries only if 503, 504, 429 " +
        "regardless of the http method");

async static Task SendRequest(
    IEazyClients clients)
{
    try
    {
        var response = await clients
            .CrudApiClient
            .GetAsync<string>(
                "errors/test");

        Console
            .WriteLine(response);
    }
    catch (Exception ex)
    {
        var mex = ex
            .Message
            .Replace(
                "[",
                $"{Environment.NewLine}[")
            .Replace(
                "(",
                $"{Environment.NewLine}(")
            .Replace(
                "{",
                $"{Environment.NewLine}{{")
            .Replace(
                "})",
                $"}}){Environment.NewLine}"); 

        Console
            .WriteLine(mex);
    }
}

Console
    .WriteLine(
        "For the first request we want a success after a retry, " +
        "waiting until the 55th second to send the request");

Console
    .WriteLine();

while (DateTime.UtcNow.Second < 55)
{
    await Task.Delay(100);
}

await SendRequest(clients);

Console
    .WriteLine();

Console
    .WriteLine();

Console
    .WriteLine(
        "For the second request we want a direct failure with no retries, " +
        "waiting until the 14th second to send the request");

Console
    .WriteLine();

while (DateTime.UtcNow.Second < 14)
{
    await Task.Delay(100);
}

await SendRequest(clients);

Console
    .WriteLine();

Console
    .WriteLine();

Console
    .WriteLine(
        "For the third request we aim to fail all the attempts, " +
        $"waiting until the 20th second to send the request");

Console
    .WriteLine();

while (DateTime.UtcNow.Second < 20)
{
    await Task.Delay(100);
}

await SendRequest(clients);

Console
    .WriteLine();

Console
    .WriteLine();

Console
    .WriteLine(
        "Press a key to quit!");

_ = Console
    .ReadKey(intercept: true);
