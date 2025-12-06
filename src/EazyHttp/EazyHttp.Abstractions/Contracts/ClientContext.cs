namespace EazyHttp.Contracts;

/// <summary>
/// Context passed to runtime resolvers (DI + client name).
/// </summary>
/// <remarks>
/// Initializes a new instance of the <see cref="ClientContext"/> struct with the specified client name and services.
/// </remarks>
public readonly struct ClientContext(
  string clientName, 
  IServiceProvider serviceProvider)
{
  /// <summary>
  /// Gets the name of the client.
  /// </summary>
  public string ClientName { get; } = clientName;

  /// <summary>
  /// Gets the service provider.
  /// </summary>
  public IServiceProvider ServiceProvider { get; } = serviceProvider;
}
