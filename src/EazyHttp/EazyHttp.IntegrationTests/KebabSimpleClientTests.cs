namespace EazyHttp.IntegrationTests;

[Collection("TestServer")]
public class KebabSimpleClientTests
{
  [Fact]
  public async Task PostAsync_json_body_as_dto()
  {
    // Arrange
    var body = new EchoDto("456");

    // Act & Assert
    await TestApiHost.Send<IKebabSimpleClient>(async client =>
    {
      var result = await client.PostAsync<EchoDto>(
        route: "echo/kebab-json",
        body: body);

      Assert.NotNull(result);
      Assert.Equal("456", result.EchoValue);
    });
  }
}