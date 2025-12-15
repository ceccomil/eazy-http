namespace EazyHttp.IntegrationTests;

[Collection("TestServer")]
public class HeadersClientTests
{
  [Fact]
  public async Task PostUrlEncodedFormAsync_as_dto()
  {
    // Arrange
    var elements = new[]
    {
      new KeyValuePair<string, string?>("value", "456")
    };

    await TestApiHost.Send<IHeadersClient>(async client =>
    {
      // Act
      var result = await client.PostUrlEncodedFormAsync<EchoDto>(
        route: "echo/form",
        additionalHeaders: [new("X-Flag-For-Form", null)],
        elements: elements);

      // Assert
      Assert.NotNull(result);
      Assert.Equal("456", result!.EchoValue);
    });
  }

  [Fact]
  public async Task PostUrlEncodedFormAsync_as_dto_with_persisted_headers()
  {
    // Arrange
    var elements = new[]
    {
      new KeyValuePair<string, string?>("value", "456")
    };

    await TestApiHost.Send<IPersistedHeadersClient>(async client =>
    {
      // Act
      var result = await client.PostUrlEncodedFormAsync<EchoDto>(
        route: "echo/form",
        elements: elements);

      // Assert
      Assert.NotNull(result);
      Assert.Equal("456", result!.EchoValue);
    });
  }
}
