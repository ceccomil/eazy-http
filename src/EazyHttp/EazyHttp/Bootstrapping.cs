using EazyHttp.Contracts;
using EazyHttp.Exceptions;
using EazyHttp.Runtime;
using System.Collections.Immutable;
using System.ComponentModel;
using System.Net;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;

namespace EazyHttp;

/// <summary>
/// Provides extension methods for configuring Eazy HTTP clients within an application's dependency injection container.
/// </summary>
/// <remarks>This class contains static methods intended to simplify the registration and configuration of Eazy
/// HTTP clients using the IServiceCollection interface. All members are static and should be accessed via extension
/// method syntax.</remarks>
public static class Bootstrapping
{
  /// <summary>
  /// Configures Eazy HTTP clients and registers related services in the dependency injection container.
  /// </summary>
  /// <remarks>Call this method during application startup to enable Eazy HTTP client features. This method is
  /// typically used in the ConfigureServices method of your application's startup class.</remarks>
  /// <param name="services">The service collection to which Eazy HTTP client services will be added.</param>
  /// <param name="config">An optional delegate to configure Eazy HTTP client options. If not specified, default options are used.</param>
  /// <returns>The same service collection instance, with Eazy HTTP client services configured.</returns>
  public static IServiceCollection ConfigureEazyHttpClients(
    this IServiceCollection services,
    Action<EazyClientOptions>? config = default)
  {
    config ??= delegate { };

    return services.Configure(config);
  }
}