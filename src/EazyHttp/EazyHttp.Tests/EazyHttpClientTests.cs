﻿namespace EazyHttp.Tests;

[Collection("EazyHttpClient Tests")]
public class EazyHttpClientTests
{
    private readonly Mock<HttpMessageHandler> _handlerMock = new();
    private readonly Mock<IOptions<EazyClientOptions>> _optionsMock = new();
    private readonly CancellationToken _token = CancellationToken.None;
    private readonly HttpQuery _query = new();
    private readonly Dictionary<string, string> _body = [];
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

    public class TestHttpClientHandler : HttpClientHandler
    {
        public TestHttpClientHandler()
        {
            AutomaticDecompression = DecompressionMethods.GZip;
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

        response
            .Content
            .Headers
            .ContentType = new("application/json");

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

    private void DefaultFailureSetup(
        int attempts = 3)
    {
        var opts = new EazyClientOptions();
        opts.Retries.Add(
            "TestHttpClient",
            new()
            {
                MaxAttempts = attempts,
                StatusCodeMatchingCondition = (status, method) =>
                {
                    if (method == HttpMethod.Get &&
                        status == HttpStatusCode.ServiceUnavailable)
                    {
                        return true;
                    }

                    return false;
                }
            });

        _optionsMock
            .Setup(x => x.Value)
            .Returns(opts);

        var response = new HttpResponseMessage
        {
            StatusCode = HttpStatusCode.ServiceUnavailable,
            Content = new StringContent(
                @"{""result"": ""KO""}")
        };

        response
            .Content
            .Headers
            .ContentType = new("application/json");

        _handlerMock
            .Protected()
            .Setup<Task<HttpResponseMessage>>(
                "SendAsync",
                ItExpr.IsAny<HttpRequestMessage>(),
                ItExpr.IsAny<CancellationToken>())
            .ReturnsAsync(response);

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
    public async Task When_performing_get_no_returns()
    {
        // Arrange
        DefaultSetup();

        // Act
        await _httpClient
            .GetAsync(
                "/test",
                _query,
                cancellationToken: _token);

        // Assert
        _httpClient
            .ResponseCode
            .Should()
            .Be(200);
    }

    [Fact]
    public async Task When_failing_get_no_returns()
    {
        // Arrange
        DefaultSetup();

        var response = new HttpResponseMessage
        {
            StatusCode = HttpStatusCode.BadRequest,
            Content = new StringContent("Fail")
        };

        _handlerMock
            .Protected()
            .Setup<Task<HttpResponseMessage>>(
                "SendAsync",
                ItExpr.IsAny<HttpRequestMessage>(),
                ItExpr.IsAny<CancellationToken>())
            .ReturnsAsync(response);

        // Act
        var ex = await Assert
            .ThrowsAsync<FailedRequestException>(() =>
            {
                return _httpClient
                .GetAsync(
                    "/test",
                    _query,
                    cancellationToken: _token);
            });

        // Assert
        ex
            .Should()
            .NotBeNull();
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
    public async Task When_performing_put_no_returns()
    {
        // Arrange
        DefaultSetup();

        // Act
        await _httpClient
            .PutAsync(
                "/1",
                _body,
                new AuthenticationHeaderValue(
                    "bearer",
                    "token"),
                new RequestHeader[]
                {
                    new("XTest", "Value")
                },
                cancellationToken: _token);

        // Assert
        _httpClient
            .ResponseCode
            .Should()
            .Be(200);

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
    public async Task When_performing_post_no_returns()
    {
        // Arrange
        DefaultSetup();

        // Act
        await _httpClient
            .PostAsync(
                null!,
                null!,
                cancellationToken: _token);

        // Assert
        _httpClient
            .ResponseCode
            .Should()
            .Be(200);
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
    public async Task When_performing_delete_no_returns()
    {
        // Arrange
        DefaultSetup();

        // Act
        await _httpClient
            .DeleteAsync(
                "/test",
                _query,
                cancellationToken: _token);

        // Assert
        _httpClient
            .ResponseCode
            .Should()
            .Be(200);
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
    public async Task When_performing_patch_no_returns()
    {
        // Arrange
        DefaultSetup();

        // Act
        await _httpClient
            .PatchAsync(
                "/testId",
                _body,
                cancellationToken: _token);

        // Assert
        _httpClient
            .ResponseCode
            .Should()
            .Be(200);
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
    public async Task When_performing_postForm_no_returns()
    {
        // Arrange
        DefaultSetup();

        // Act
        await _httpClient
            .PostFormAsync(
                "/test",
                new FormElement[]
                {
                    new("Field1", new StringContent(@"{{""aJson"": ""value""}}")),
                    new("Field2", new ByteArrayContent(Guid.NewGuid().ToByteArray()))
                    {
                       FileName = "fileName"
                    }
                },
                cancellationToken: _token);

        // Assert
        _httpClient
            .ResponseCode
            .Should()
            .Be(200);
    }

    [Fact]
    public async Task When_failing_postForm_no_returns()
    {
        // Arrange
        DefaultSetup();

        var response = new HttpResponseMessage
        {
            StatusCode = HttpStatusCode.BadRequest,
            Content = new StringContent("Fail")
        };

        _handlerMock
            .Protected()
            .Setup<Task<HttpResponseMessage>>(
                "SendAsync",
                ItExpr.IsAny<HttpRequestMessage>(),
                ItExpr.IsAny<CancellationToken>())
            .ReturnsAsync(response);

        // Act
        var ex = await Assert.ThrowsAsync<FailedRequestException>(() =>
        {
            return _httpClient
                .PostFormAsync(
                    "/test",
                    new FormElement[]
                    {
                    new("Field1", new StringContent(@"{{""aJson"": ""value""}}")),
                    new("Field2", new ByteArrayContent(Guid.NewGuid().ToByteArray()))
                    {
                       FileName = "fileName"
                    }
                    },
                    cancellationToken: _token);
        });

        // Assert
        ex
            .Should()
            .NotBeNull();
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

    [Fact]
    public async Task When_performing_postUrlEncoded_no_returns()
    {
        // Arrange
        DefaultSetup();

        // Act
        await _httpClient
            .PostUrlEncodedFormAsync(
                "/test",
                new Dictionary<string, string?>()
                {
                    { "element1", "value1" },
                    { "element2", "value2" }
                },
                cancellationToken: _token);

        // Assert
        _httpClient
            .ResponseCode
            .Should()
            .Be(200);
    }

    [Fact]
    public async Task When_retries_are_made()
    {
        // Arrange
        DefaultFailureSetup(2);

        // Act
        var exception = await Assert
            .ThrowsAsync<AggregateException>(
                () => _httpClient
                    .GetAsync<TestResponse>(
                        "/test",
                        cancellationToken: _token));

        // Assert
        exception
            .InnerExceptions
            .All(x => x is FailedRequestException)
            .Should()
            .BeTrue();
    }

    [Fact]
    public async Task When_retries_are_cancelled()
    {
        // Arrange
        DefaultFailureSetup(10);

        var cancellation = new CancellationTokenSource();

        _ = Task
            .Run(async () =>
            {
                await Task.Delay(10_000);
                cancellation.Cancel();
            });

        // Act

        var exception = await Assert
            .ThrowsAsync<TaskCanceledException>(
                () => _httpClient
                    .GetAsync<TestResponse>(
                        "/test",
                        cancellationToken:
                            cancellation.Token));

        // Assert
        exception
            .Should()
            .NotBeNull();
    }

    [Fact]
    public async Task When_attaching_an_http_handler()
    {
        var services = new ServiceCollection()
            .AddTransient<TestHttpClientHandler>();

        services
            .AddHttpClient<TestHttpClient>()
            .ConfigurePrimaryHttpMessageHandler<TestHttpClientHandler>();

        var sp = services
            .BuildServiceProvider()
            .CreateScope()
            .ServiceProvider;

        var client = sp
            .GetRequiredService<TestHttpClient>();

        var result = await client
            .GetAsync<string>(
                "https://www.google.com");

        result
            .Should()
            .NotBeNullOrEmpty();
    }

    [Fact]
    public async Task When_checking_response_headers()
    {
        var services = new ServiceCollection();

        List<KeyValuePair<Guid, string>> requests =
        [
            new(Guid.Parse("00000000-0000-0000-0000-000000000001"), "https://www.google.com"),
            new(Guid.Parse("00000000-0000-0000-0000-000000000002"), "https://www.nuget.org"),
            new(Guid.Parse("00000000-0000-0000-0000-000000000003"), "https://www.github.com")
        ];

        services
            .AddHttpClient<TestHttpClient>();

        var sp = services
            .BuildServiceProvider()
            .CreateScope()
            .ServiceProvider;

        var client = sp
            .GetRequiredService<TestHttpClient>();

        List<Task> tasks = [];

        for (var i = 0; i < requests.Count; i++)
        {
            tasks.Add(client
                .GetAsync<string>(
                    requests[i].Value,
                    requestId: requests[i].Key));
        }

        var maxTime = DateTime
            .UtcNow
            .AddSeconds(60);

        while (DateTime.UtcNow < maxTime &&
            client.ResponseResults.Count < requests.Count)
        {
            await Task.Delay(50);
        }

        client
            .ResponseResults
            .Count
            .Should()
            .Be(requests.Count);

        client
            .ResponseResults
            .All(x => x.ResponseHeaders is not null);
    }

    [Fact]
    public async Task When_checking_response_on_failures()
    {
        // Arrange     
        var services = new ServiceCollection();

        services
            .Configure<EazyClientOptions>(opts =>
            {
                opts.Retries.Add("TestHttpClient", new()
                {
                    MaxAttempts = 2,
                    StatusCodeMatchingCondition = (status, method) =>
                    {
                        if (method == HttpMethod.Get &&
                            status == HttpStatusCode.Unauthorized)
                        {
                            return true;
                        }

                        return false;
                    }
                });
            })
            .AddHttpClient<TestHttpClient>();

        var sp = services
            .BuildServiceProvider()
            .CreateScope()
            .ServiceProvider;

        var client = sp
            .GetRequiredService<TestHttpClient>();

        Exception error = null!;

        // Act
        try
        {
            _ = await client.GetAsync<object>(
                "https://api.ote-godaddy.com/v1/domains");
        }
        catch (Exception ex)
        {
            error = ex;
        }

        // Assert
        error
            .GetType()
            .Should()
            .Be<AggregateException>();

        client
            .ResponseResults
            .Count
            .Should()
            .Be(1);
    }
}
