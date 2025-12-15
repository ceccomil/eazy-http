using EazyHttp.Exceptions;

namespace EazyHttp.IntegrationTests;

[Collection("TestServer")]
public class RetriesClientTests
{
  [Fact]
  public async Task PutAsync_json_body_as_dto()
  {
    // Arrange
    var body = new EchoDto("456");

    // Act & Assert
    await TestApiHost.Send<IRetriesClient>(async client =>
    {
      var result = await client.PutAsync<EchoDto>(
        route: "echo/put-json",
        body: body);

      Assert.NotNull(result);
      Assert.Contains("456", result.EchoValue);
      Assert.Contains("Success after: 3 attempts.", result.EchoValue);
    });
  }

  [Fact]
  public async Task PutAsync_json_body_as_dto_when_max_attempts_exceeded()
  {
    // Arrange
    var body = new EchoDto("456");

    // Act & Assert
    await TestApiHost.Send<INoResolverRetriesClient>(async client =>
    {
      Exception? check = null;
      try
      {
        var result = await client.PutAsync<EchoDto>(
          route: "echo/put-json",
          body: body);
      }
      catch (FailedRequestException ex)
      {
        Assert.Contains("Error after: 2 attempts", ex.ResponseContent);
        check = ex;
      }
      
      Assert.NotNull(check);
    });
  }
}
