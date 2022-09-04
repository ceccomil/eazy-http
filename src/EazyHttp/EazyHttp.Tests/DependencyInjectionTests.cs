using EazyHttp;
using EazyHttp.Contracts;
using EazyHttp.HttpClients;

namespace EazyHttp.Tests;

[Collection("Dependency Injection Tests")]
public class DependencyInjectionTests
{
    [Fact]
    public async Task Test1()
    {
        var services = new ServiceCollection()
            .ConfigureEazyHttpClients(opts =>
            {
                opts
                    .EazyHttpClients
                    .Add(new("WeatherHttp", "https://localhost:7004/"));

                opts
                    .EazyHttpClients
                    .Add(new("WeatherHttp1", "https://localhost:7004/"));

                opts
                    .PersistentHeaders
                    .Add(
                        "WeatherHttp",
                        new RequestHeader[]
                        {
                            new("X-Persit1", "Persist 1 value"),
                            new("X-Persit2", "Persist 2 value"),
                        });

            })
            .AddEazyHttpClients();

        using var sp = services.BuildServiceProvider();
        using var scope = sp.CreateScope();

        //var test = Logs.Collection;

        var clients = scope
            .ServiceProvider
            .GetRequiredService<IEazyClients>();

        var result = await clients
            .WeatherHttp
            .GetAsync<IEnumerable<WeatherForecast>>(
                "/WeatherForecast",
                additionalHeaders: new RequestHeader[]
                {
                    new("X-First", "This is the value 1")
                });

        result = await clients
            .WeatherHttp1
            .GetAsync<IEnumerable<WeatherForecast>>(
                "/WeatherForecast",
                additionalHeaders: new RequestHeader[]
                {
                    new("X-Second", "This is the value 2")
                });

        result = await clients
            .WeatherHttp
            .GetAsync<IEnumerable<WeatherForecast>>(
                "/WeatherForecast");

        result = await clients
            .WeatherHttp1
            .GetAsync<IEnumerable<WeatherForecast>>(
                "/WeatherForecast");
    }
}

public class WeatherForecast
{
    public DateTime Date { get; set; }

    public int TemperatureC { get; set; }

    public int TemperatureF { get; set; }

    public string? Summary { get; set; }
}