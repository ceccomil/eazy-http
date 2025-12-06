namespace EazyHttp.Contracts;

/// <summary>
/// The HTTP header that may be specified in a client request.
/// </summary>
public sealed record RequestHeader
{
  /// <summary>
  /// Gets the name of the HTTP header.
  /// </summary>
  public string Name { get; }
  
  /// <summary>
  /// Gets the value of the HTTP header.
  /// </summary>
  public string? Value { get; }

  /// <summary>
  /// Initializes a new instance of the RequestHeader class with the specified header name and value.
  /// </summary>
  /// <param name="name">The name of the HTTP header. Cannot be null or empty.</param>
  /// <param name="value">The value of the HTTP header, or null if the header has no value.</param>
  public RequestHeader(
    string name, 
    string? value)
  {
    Name = name;
    Value = value;
  }
};
