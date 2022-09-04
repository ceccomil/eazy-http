using EazyHttp;
using EazyHttp.HttpClients;

namespace EazyHttp.Tests;

[Collection("Dependency Injection Tests")]
public class DependencyInjectionTests
{
    [Fact]
    public async Task Test1()
    {
        var services = new ServiceCollection()
            .ConfigureEazyHttpClients()
            //.ConfigureEazyHttpClients(opts =>
            //{
            //    opts.EazyHttpClients = new()
            //    {
            //        new("DefaultHttp", ""),
            //        new("DefaultHttp1", "baseddr1"),
            //        new("DefaultHttp2", "baseAddr2")
            //    };
            //})
            .AddEazyHttpClients();

        using var sp = services.BuildServiceProvider();
        using var scope = sp.CreateScope();

        //var test = Logs.Collection;

        var apiUrl = "https://localhost:7175/WeatherForecast";

        var client = scope.ServiceProvider.GetRequiredService<ISharedHttpClient>();

        var result = await client.GetAsync<IEnumerable<WeatherForecast>>(apiUrl);
    }
}

public class WeatherForecast
{
    public DateTime Date { get; set; }

    public int TemperatureC { get; set; }

    public int TemperatureF { get; set; }

    public string? Summary { get; set; }
}