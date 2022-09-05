namespace EazyHttp.Tests;

[Collection("EazyHttpClient Tests")]
public class EazyHttpClientTests
{
    private readonly Mock<HttpMessageHandler> _handlerMock = new();
    private readonly Mock<IOptions<EazyClientOptions>> _optionsMock = new();
    private readonly CancellationToken _token = CancellationToken.None;
    private readonly HttpQuery _query = new();
    private readonly Dictionary<string, string> _body = new();
    private EazyHttpClientBase _httpClient = null!;

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

    private void DefaultSetup()
    {
        _optionsMock
            .Setup(x => x.Value)
            .Returns(new EazyClientOptions());

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

        _query
            .AddParam(
                new("Field1", "Value1"));

        _query
            .AddParam(
                new("Field2", "Value2"));

        _body
            .Add("Key1", "Val1");

        _body
            .Add("Key2", "Val2");

        _httpClient = new TestHttpClient(
            new(
                _handlerMock.Object),
                _optionsMock.Object);
    }

    [Fact]
    public async Task When_performing_get()
    {
        // Arrange
        DefaultSetup();
    
        // Act
        var testResponse = await _httpClient
            .GetAsync<TestResponse>(
                "/test",
                _query,
                cancellationToken: _token)
            ?? throw new NullReferenceException();

        // Assert
        testResponse
            .Result
            .Should()
            .Be("OK");
    }

    [Fact]
    public async Task When_performing_put()
    {
        // Arrange
        DefaultSetup();

        // Act
        var testResponse = await _httpClient
            .PutAsync<TestResponse>(
                "/1",
                _body,
                new AuthenticationHeaderValue(
                    "bearer",
                    "token"),
                new RequestHeader[]
                {
                    new("XTest", "Value")
                },
                cancellationToken: _token)
            ?? throw new NullReferenceException();

        // Assert
        testResponse
            .Result
            .Should()
            .Be("OK");

       _httpClient
            .Headers
            .Any()
            .Should()
            .BeFalse(
                because: "All temporary headers (including auth)" +
                " have been removed after the request!");
    }

    [Fact]
    public async Task When_performing_post()
    {
        // Arrange
        DefaultSetup();

        // Act
        var testResponse = await _httpClient
            .PostAsync<TestResponse>(
                null!,
                null!,
                cancellationToken: _token)
            ?? throw new NullReferenceException();

        // Assert
        testResponse
            .Result
            .Should()
            .Be("OK");
    }

    [Fact]
    public async Task When_performing_delete()
    {
        // Arrange
        DefaultSetup();

        // Act
        var testResponse = await _httpClient
            .DeleteAsync<TestResponse>(
                "/test",
                _query,
                cancellationToken: _token)
            ?? throw new NullReferenceException();

        // Assert
        testResponse
            .Result
            .Should()
            .Be("OK");
    }

    [Fact]
    public async Task When_performing_patch()
    {
        // Arrange
        DefaultSetup();

        // Act
        var testResponse = await _httpClient
            .PatchAsync<TestResponse>(
                "/testId",
                _body,
                cancellationToken: _token)
            ?? throw new NullReferenceException();

        // Assert
        testResponse
            .Result
            .Should()
            .Be("OK");
    }

    [Fact]
    public async Task When_performing_postForm()
    {
        // Arrange
        DefaultSetup();

        // Act
        var testResponse = await _httpClient
            .PostFormAsync<TestResponse>(
                "/test",
                new FormElement[]
                {
                    new("Field1", new StringContent(@"{{""aJson"": ""value""}}")),
                    new("Field2", new ByteArrayContent(Guid.NewGuid().ToByteArray()))
                    {
                       FileName = "fileName"
                    }
                },
                cancellationToken: _token)
            ?? throw new NullReferenceException();

        // Assert
        testResponse
            .Result
            .Should()
            .Be("OK");
    }

    [Fact]
    public async Task When_performing_postUrlEncoded()
    {
        // Arrange
        DefaultSetup();

        // Act
        var testResponse = await _httpClient
            .PostUrlEncodedFormAsync<TestResponse>(
                "/test",
                new Dictionary<string, string?>()
                {
                    { "element1", "value1" },
                    { "element2", "value2" }
                },
                cancellationToken: _token)
            ?? throw new NullReferenceException();

        // Assert
        testResponse
            .Result
            .Should()
            .Be("OK");
    }
}
