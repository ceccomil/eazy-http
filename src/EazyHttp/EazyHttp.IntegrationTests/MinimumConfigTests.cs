namespace EazyHttp.IntegrationTests;

[Collection("TestServer")]
public class MinimumConfigTests
{
  [Fact]
  public async Task GetAsync_as_json()
  {
    // Arrange
    var query = new HttpQuery();
    query.AddParam(new HttpQueryParam("value", "123"));

    using TestServer server = TestApiHost
      .Create();

    var serverHost = server
      .Services
      .GetRequiredService<IHost>();

    await serverHost.StartAsync();

    try
    {
      var services = new ServiceCollection()
        .ConfigureEazyHttpClients()
        .AddEazyHttpClients();

      var serviceProvider = services.BuildServiceProvider();

      var client = serviceProvider
        .GetRequiredService<ISharedClient>();

      var result = await client.GetAsync<EchoDto>(
        route: "http://localhost:5000/api/echo/query",
        query: query);

      // Assert
      Assert.NotNull(result);
      Assert.Equal(
        new("123"),
        result);
    }
    catch (Exception ex)
    {
      Assert.Fail(ex.Message);
    }
    finally
    {
      await serverHost.StopAsync();
    }
  }
}
