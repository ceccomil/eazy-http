namespace EazyHttp.Contracts;

/// <summary>
/// Definition of an HTTP client
/// </summary>
public sealed record HttpClientDefinition
{
  /// <summary>
  /// Http client name
  /// </summary>
  /// <remarks>
  /// This must be a literal string. It will become the named HttpClient registered in the DI container.
  /// </remarks>
  public string Name { get; }

  /// <summary>
  /// Http client base address
  /// </summary>
  /// <remarks>
  /// This can be null, a literal string, or a variable. If variable, it must be resolvable by the generated named HttpClient.
  /// </remarks>
  public string? BaseAddress { get; }

  /// <summary>
  /// If true, methods of the client can accept optional headers
  /// </summary>
  public bool OptionalHeadersInMethods { get; }

  /// <summary>
  /// Initializes a new instance of the HttpClientDefinition class with the specified name and optional base address.
  /// </summary>
  public HttpClientDefinition(
    string name, 
    string? baseAddress,
    bool optionalHeadersInMethods)
  {
    Name = name;
    BaseAddress = baseAddress;
    OptionalHeadersInMethods = optionalHeadersInMethods;
  }

  /// <inheritdoc/>
  public override string ToString()
  {
    var baseAddress = "";
    var optionalHeaders = "";

    if (!string.IsNullOrWhiteSpace(BaseAddress))
    {
      baseAddress = $" ({BaseAddress})";
    }

    if (OptionalHeadersInMethods)
    {
      optionalHeaders = " [Optional Headers Enabled]";
    }

    return Name + baseAddress + optionalHeaders;
  }
}
