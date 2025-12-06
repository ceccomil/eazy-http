using EazyHttp;
using EazyHttp.Contracts;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;

namespace Tests.Data;

internal class HttpClientConfiguration
{
  internal static readonly JsonSerializerOptions _jsonOpts = new()
  {
    PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
    WriteIndented = true
  };

  internal static readonly string Url = "https://api.example.com";

  public static void Main()
  {
    var services = new ServiceCollection()
      .ConfigureEazyHttpClients(x =>
      {
        x.NamespacePrefix = "MyCustom.Namespace";

        x.Clients.Add(new("Test1", "https://test", false));

        x
          .SerializerOptions.Add(
            "Test1",
            Tests.Data.HttpClientConfiguration._jsonOpts);

        x.Clients.Add(
          new HttpClientDefinition(
            "Test2",
            Tests.Data.HttpClientConfiguration.Url,
            true));

        x.Clients.Add(new("Test3", null, false));

        x.Clients.Add(new("Test4", null, true));

        // Retries
        x.Retries.Add("Test2", new()
        {
          MaxAttempts = 5,
          StatusCodeMatchingCondition = (code, method) =>
          {
            if (code is System.Net.HttpStatusCode.Continue &&
            method == HttpMethod.Post)
            {
              return true;
            }

            return false;
          }
        });

        // Encodings
        x.Encodings.Add("Test3", Encoding.ASCII);

        // PersistentHeaders
        x.PersistentHeaders.Add("Test4",
          new[] { new RequestHeader("X-Api-Key", "123") });

        x.HttpClientHandlerTypeNames.Add("Test1",
          "Tests.Data.CustomMessageHandler");

        x.HttpClientHandlerTypeNames.Add("Test3",
          nameof(CustomMessageHandler));

        x.ResolveSerializer = context =>
        {
          if (context.ClientName == "Test2")
          {
            return new(JsonSerializerDefaults.Web)
            {
              PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
              WriteIndented = true
            };
          }

          return new(JsonSerializerDefaults.Web);
        };

      });
  }
}

internal class CustomMessageHandler : HttpClientHandler
{
  protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
  {
    if (request.Headers.Contains("X-Custom-Header"))
    {
      request.Headers.Remove("X-Custom-Header");
    }

    request.Headers.Add("X-Custom-Header", Guid.NewGuid().ToString());

    return base.SendAsync(request, cancellationToken);
  }
}
