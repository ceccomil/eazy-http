using Microsoft.Extensions.Options;
using Moq.Protected;
using System.Net;

namespace EazyHttp.Tests;

[Collection("EazyHttpClient Tests")]
public class EazyHttpClientTests
{
    private readonly Mock<HttpMessageHandler> _handlerMock = new();

    private readonly Mock<IOptions<EazyClientOptions>> _optionsMock = new();

    public class TestHttpClient : EazyHttpClientBase
    {
        public TestHttpClient(
            HttpClient httpClient,
            IOptions<EazyClientOptions> options)
            : base(httpClient, options)
        {
            httpClient.BaseAddress = new("https://locahost/test/");
        }
    }

    public class TestResponse
    {
        public string? Result { get; set; }
    }

    [Fact]
    public async Task When_performing_get()
    {
        // Arrange
        _optionsMock
            .Setup(x => x.Value)
            .Returns(new EazyClientOptions());

        var query = new HttpQuery();
        query.AddParam(new("Field1", "Value1"));
        query.AddParam(new("Field2", "Value2"));

        var token = new CancellationTokenSource()
            .Token;

        var response = new HttpResponseMessage
        {
            StatusCode = HttpStatusCode.OK,
            Content = new StringContent(
                @"{""result"": ""OK""}")
        };

        _handlerMock
            .Protected()
            .Setup<Task<HttpResponseMessage>>(
                "SendAsync",
                ItExpr.IsAny<HttpRequestMessage>(),
                ItExpr.IsAny<CancellationToken>())
            .ReturnsAsync(response);

        var client = new TestHttpClient(
            new(
                _handlerMock.Object),
            _optionsMock.Object);

        // Act
        var testResponse = await client
            .GetAsync<TestResponse>(
                "/test",
                query,
                cancellationToken: token)
            ?? throw new NullReferenceException();

        // Assert
        testResponse
            .Result
            .Should()
            .Be("OK");
    }
}
