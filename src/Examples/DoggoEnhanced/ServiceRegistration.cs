namespace DoggoEnhanced;

public static class ServiceRegistration
{
  //private static IServiceCollection ConfigureHttpClients(
  //    this IServiceCollection services)
  //{
  //    services
  //        .ConfigureEazyHttpClients(opts =>
  //        {
  //            opts
  //                .NameSpacePrefix = "DoggoEnhanced";

  //            opts
  //                .EazyHttpClients
  //                .Add(new(
  //                    "ComputerVision",
  //                    "https://<computer vision service name>.cognitiveservices.azure.com/vision/v3.2/"));

  //            opts
  //                .EazyHttpClients
  //                .Add(new(
  //                    "DeepAi",
  //                    "https://api.deepai.org/api"));

  //            opts
  //                .SerializersOptions
  //                .Add(
  //                    "DeepAi",
  //                    new(JsonSerializerDefaults.Web)
  //                    {
  //                        PropertyNamingPolicy = new SnakeCasePolicy()
  //                    });

  //            opts
  //                .PersistentHeaders
  //                .Add(
  //                    "ComputerVision",
  //                    new RequestHeader[]
  //                    {
  //                        new(
  //                            "Ocp-Apim-Subscription-Key",
  //                            "<computer vision subscription key>")
  //                    });

  //            opts
  //                .PersistentHeaders
  //                .Add(
  //                    "DeepAi",
  //                    new RequestHeader[]
  //                    {
  //                        new(
  //                            "api-key",
  //                            "quickstart-QUdJIGlzIGNvbWluZy4uLi4K")
  //                    });

  //        });

  //    return services;
  //}

  public static IServiceCollection RegisterServices(
      this IServiceCollection services)
  {
    services
        .ConfigureHttpClients()
        .AddEazyHttpClients()
        .AddTransient<IRandomDog, RandomDog>()
        .DoggoEnhancedAddEazyHttpClients()
        .AddTransient<IImageAnalysis, ImageAnalysis>();

    return services;
  }
}
