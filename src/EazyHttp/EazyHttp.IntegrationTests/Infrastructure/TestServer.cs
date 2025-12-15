namespace EazyHttp.IntegrationTests.Infrastructure;

internal static class TestApiHost
{
  private static JsonSerializerOptions SerializerOptions { get; } = new(JsonSerializerDefaults.Web)
  {
    PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
    WriteIndented = true
  };

  private static JsonSerializerOptions KebabUpperSerializerOptions { get; } = new(JsonSerializerDefaults.General)
  {
    PropertyNamingPolicy = JsonNamingPolicy.KebabCaseUpper
  };

  private static JsonSerializerOptions SnakeLowerSerializerOptions { get; } = new(JsonSerializerDefaults.General)
  {
    PropertyNamingPolicy = JsonNamingPolicy.SnakeCaseLower
  };

  public static async Task Send<TClient>(
    Func<TClient, Task> actAndAssert)
    where TClient : notnull
  {
    using TestServer server = TestApiHost
      .Create();

    var serverHost = server
      .Services
      .GetRequiredService<IHost>();

    await serverHost.StartAsync();

    try
    {
      var services = new ServiceCollection()
        .AddTransient<IAuthenticatorForTests, AuthenticatorForTests>()
        .AddEazyHttpConfig();

      var serviceProvider = services.BuildServiceProvider();

      var client = serviceProvider
        .GetRequiredService<TClient>();

      await actAndAssert(client);
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

  public static TestServer Create()
  {
    var builder = WebApplication
      .CreateBuilder();

    builder
      .Services
      .AddSingleton<IRetriesState, RetriesState>()
      .Configure<CaptainLoggerOptions>(opts =>
      {
        opts.TimeIsUtc = true;
        opts.LogRecipients = Recipients.Console;
      })
      .AddLogging(builder =>
      {
        builder
          .ClearProviders()
          .AddCaptainLogger()
          .AddFilter("Microsoft", LogLevel.Debug);
      })
      .ConfigureHttpJsonOptions(options =>
      {
        options.SerializerOptions.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;
        options.SerializerOptions.WriteIndented = true;
      })
      .AddRouting();

    var app = builder.Build();

    app.SetupEndpoints();

    return new TestServer(app.Services);
  }

  private static void SetupEndpoints(this WebApplication app)
  {
    var api = app.MapGroup("api");

    api.MapGet("echo/query", async context =>
    {
      var query = context.Request.Query;

      var echo = new EchoDto(
        query["value"].ToString());

      await context.Response.WriteAsJsonAsync(
        echo, 
        options: SerializerOptions);
    });

    api.MapPost("echo/kebab-json", async context =>
    {
      var dto = await context
        .Request
        .ReadFromJsonAsync<EchoDto>(KebabUpperSerializerOptions);

      if (dto is null)
      {
        context.Response.StatusCode = StatusCodes.Status400BadRequest;
        return;
      }

      await context.Response.WriteAsJsonAsync(
        dto, 
        options: SnakeLowerSerializerOptions);
    });

    api.MapPost("echo/form", async context =>
    {
      if (!context.Request.HasFormContentType)
      {
        context.Response.StatusCode = StatusCodes.Status400BadRequest;
        return;
      }

      if (!context.Request.Headers.ContainsKey("X-Flag-For-Form"))
      {
        context.Response.StatusCode = StatusCodes.Status422UnprocessableEntity;
        return;
      }

      var form = await context.Request.ReadFormAsync();

      var echo = new EchoDto(
        form["value"].ToString());

      await context.Response.WriteAsJsonAsync(
        echo,
        options: SerializerOptions);
    });

    api.MapPost("echo/multipart", async context =>
    {
      if (!context.Request.HasFormContentType)
      {
        context.Response.StatusCode = StatusCodes.Status400BadRequest;
        return;
      }

      var form = await context.Request.ReadFormAsync();

      var fields = form
        .ToDictionary(
          kvp => kvp.Key,
          kvp => (string?)kvp.Value.ToString());

      var files = form.Files
        .Select(f => new MultipartEchoFileDto(
          Name: f.Name,
          FileName: f.FileName,
          ContentType: f.ContentType,
          Length: f.Length))
        .ToArray();

      var dto = new MultipartEchoDto(
        Fields: fields,
        Files: files);

      await context.Response.WriteAsJsonAsync(
        dto,
        options: SerializerOptions);
    });

    api.MapPatch("echo/patch-json", async context =>
    {
      var key = context
        .Request
        .Headers["X-Api-Key"]
        .FirstOrDefault();

      if (!IAuthenticatorForTests.KEY.Equals(key, StringComparison.OrdinalIgnoreCase))
      {
        context.Response.StatusCode = StatusCodes.Status401Unauthorized;
        return;
      }

      var dto = await context
        .Request
        .ReadFromJsonAsync<EchoDto>(SerializerOptions);

      if (dto?.EchoValue is null)
      {
        context.Response.StatusCode = StatusCodes.Status400BadRequest;
        return;
      }

      await context.Response.WriteAsJsonAsync(
        dto,
        options: SerializerOptions);
    });

    api.MapPut("echo/put-json", async (
      HttpContext context,
      [FromBody] EchoDto dto,
      [FromServices] IRetriesState retries) =>
    {
      retries.IncrementAttempts();
      var attempt = retries.Attempts;

      if (attempt < 3)
      {
        context.Response.StatusCode = StatusCodes.Status422UnprocessableEntity;
        return new($"Error after: {retries.Attempts} attempts");
      }

      dto = new EchoDto(
        $"{dto!.EchoValue}{Environment.NewLine}" +
        $"Success after: {retries.Attempts} attempts.");

      retries.ResetAttempts();

      return dto;
    });
  }
}

internal sealed record EchoDto(string EchoValue);

internal sealed record MultipartEchoDto(
  Dictionary<string, string?> Fields,
  MultipartEchoFileDto[] Files);

internal sealed record MultipartEchoFileDto(
  string Name,
  string FileName,
  string ContentType,
  long Length);
