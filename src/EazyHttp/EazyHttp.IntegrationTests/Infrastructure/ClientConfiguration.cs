namespace EazyHttp.IntegrationTests.Infrastructure;

internal static class ClientConfiguration
{
  public static IServiceCollection AddEazyHttpConfig(
    this IServiceCollection services)
  {
    services
      .SetHeadersClientOptions()
      .SetPersistedHeadersClientOptions()
      .SetSimpleClientOptions()
      .SetKebabClientOptions()
      .SetAuthClientOptions()
      .SetRetriesClientOptions()
      .SetNoResolverRetriesClientOptions();

    return services;
  }

  private static IServiceCollection SetSimpleClientOptions(
    this IServiceCollection services)
  {
    services
      .ConfigureEazyHttpClients(options =>
      {
        options.NamespacePrefix = "IntgTests.Simple";

        options.Clients.Add(
          new HttpClientDefinition(
            name: "SimpleClient",
            baseAddress: "http://localhost:5000/api",
            optionalHeadersInMethods: false));
      })
      .AddIntgTestsSimpleClients();

    return services;
  }

  private static IServiceCollection SetHeadersClientOptions(
    this IServiceCollection services)
  {
    services
      .ConfigureEazyHttpClients(options =>
      {
        options.NamespacePrefix = "IntgTests.Headers";

        options.Clients.Add(
          new HttpClientDefinition(
            name: "HeadersClient",
            baseAddress: "http://localhost:5000/api",
            optionalHeadersInMethods: true));
      })
      .AddIntgTestsHeadersClients();

    return services;
  }

  private static IServiceCollection SetPersistedHeadersClientOptions(
    this IServiceCollection services)
  {
    services
      .ConfigureEazyHttpClients(options =>
      {
        options.NamespacePrefix = "IntgTests.PersistedHeaders";

        options.Clients.Add(
          new HttpClientDefinition(
            name: "PersistedHeadersClient",
            baseAddress: "http://localhost:5000/api",
            optionalHeadersInMethods: false));

        options.PersistentHeaders.Add(
          "PersistedHeadersClient",
          [ new("X-Flag-For-Form", "true") ]);
      })
      .AddIntgTestsPersistedHeadersClients();

    return services;
  }

  private static IServiceCollection SetKebabClientOptions(
    this IServiceCollection services)
  {
    services
      .ConfigureEazyHttpClients(options =>
      {
        options.NamespacePrefix = "IntgTests.Kebab";

        options.Clients.Add(
          new HttpClientDefinition(
            name: "KebabSimpleClient",
            baseAddress: "http://localhost:5000/api",
            optionalHeadersInMethods: false));

        options.RequestSerializerOptions.Add(
          "KebabSimpleClient",
          new JsonSerializerOptions
          {
            PropertyNamingPolicy = JsonNamingPolicy.KebabCaseUpper,
            WriteIndented = true
          });

        options.ResolveResponseSerializer = context =>
        {
          if (context.ClientName == "KebabSimpleClient")
          {
            return new JsonSerializerOptions
            {
              PropertyNamingPolicy = JsonNamingPolicy.SnakeCaseLower
            };
          }

          return null;
        };
      })
      .AddIntgTestsKebabClients();

    return services;
  }

  private static IServiceCollection SetAuthClientOptions(
    this IServiceCollection services)
  {
    services
      .ConfigureEazyHttpClients(options =>
      {
        options.NamespacePrefix = "IntgTests.Auth";

        options.Clients.Add(
          new HttpClientDefinition(
            name: "AuthClient",
            baseAddress: "http://localhost:5000/api",
            optionalHeadersInMethods: false));


        options.HttpClientHandlers.Add(
          "AuthClient",
          typeof(AuthMessageHandler));
      })
      .AddIntgTestsAuthClients();

    return services;
  }

  private static IServiceCollection SetRetriesClientOptions(
    this IServiceCollection services)
  {
    services
      .ConfigureEazyHttpClients(options =>
      {
        options.NamespacePrefix = "IntgTests.Retries";

        options.Clients.Add(
          new HttpClientDefinition(
            name: "RetriesClient",
            baseAddress: "http://localhost:5000/api",
            optionalHeadersInMethods: false));

        options.ResolveRetry = context =>
        {
          if (context.ClientName == "RetriesClient")
          {
            var policy = new RetryConfiguration()
            {
              MaxAttempts = 5,
              Seed = 200,
              StatusCodeMatchingCondition = (statusCode, httpMethod) =>
              {
                if (httpMethod == HttpMethod.Put &&
                  statusCode == HttpStatusCode.UnprocessableEntity)
                {
                  return true;
                }

                return false;
              }
            };

            return policy;
          }

          return null;
        };

      })
      .AddIntgTestsRetriesClients();

    return services;
  }

  private static IServiceCollection SetNoResolverRetriesClientOptions(
    this IServiceCollection services)
  {
    services
      .ConfigureEazyHttpClients(options =>
      {
        options.NamespacePrefix = "IntgTests.NoResolverRetries";

        options.Clients.Add(
          new HttpClientDefinition(
            name: "NoResolverRetriesClient",
            baseAddress: "http://localhost:5000/api",
            optionalHeadersInMethods: false));

        options.Retries.Add(
          "NoResolverRetriesClient",
          new()
          {
            MaxAttempts = 2,
            Seed = 400,
            StatusCodeMatchingCondition = (statusCode, httpMethod) =>
            {
              if (httpMethod == HttpMethod.Put &&
                statusCode == HttpStatusCode.UnprocessableEntity)
              {
                return true;
              }

              return false;
            }
          });

      })
      .AddIntgTestsNoResolverRetriesClients();

    return services;
  }
}