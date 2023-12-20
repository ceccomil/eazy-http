namespace EazyHttp.Tests;

[Collection("Integration Tests")]
public class IntegrationTests
{
    private async Task<TResult?> GetFromApi<TResult>(
        string route) where TResult : class
    {
        // Arrange
        using var application = new AppFactory();

        using var http = application
          .CreateClient();

        var opts = Options
            .Create<EazyClientOptions>(new());

        var client = new IntegrationClient(
            http,
            opts);

        // Act
        var result = await client
            .GetAsync<TResult>(
                route);

        return result;
    }

    [Fact]
    public async Task When_getting_a_json()
    {
        var forecast = await GetFromApi<
            IEnumerable<WeatherForecast>>(
            "integration-test/forecast");

        // Assert
        forecast
            .Should()
            .NotBeEmpty();
    }

    [Fact]
    public async Task When_getting_a_byte_array()
    {
        var fileContent = await GetFromApi<byte[]>(
            "integration-test/download");

        // Assert
        fileContent!
            .Length
            .Should()
            .Be(4_600_000);
    }

    [Fact]
    public async Task When_getting_a_string()
    {
        var stringContent = await GetFromApi<string>(
            "integration-test/download");

        // Assert
        stringContent!
            .Length
            .Should()
            .Be(4_600_000);
    }

    [Fact]
    public async Task When_getting_a_strean()
    {
        var streamContent = await GetFromApi<Stream>(
            "integration-test/download");

        // Assert
        streamContent!
            .Length
            .Should()
            .Be(4_600_000);
    }
}

file class AppFactory : WebApplicationFactory<Program>
{
    protected override IHost CreateHost(
      IHostBuilder builder)
    {
        builder
          .UseEnvironment(
            "Development");

        return base
          .CreateHost(builder);
    }
}

file class IntegrationClient(
    HttpClient httpClient,
    IOptions<EazyClientOptions> options)
    : EazyHttpClientBase(httpClient, options)
{ }
