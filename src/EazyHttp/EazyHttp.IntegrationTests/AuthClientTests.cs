namespace EazyHttp.IntegrationTests;

[Collection("TestServer")]
public class AuthClientTests
{
  [Fact]
  public async Task PatchAsync_json_body_as_dto()
  {
    // Arrange
    var body = new EchoDto("456");

    // Act & Assert
    await TestApiHost.Send<IAuthClient>(async client =>
    {
      var result = await client.PatchAsync<EchoDto>(
        route: "echo/patch-json",
        body: body);

      Assert.NotNull(result);
      Assert.Equal("456", result.EchoValue);
    });
  }
}