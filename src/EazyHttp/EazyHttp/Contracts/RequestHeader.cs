namespace EazyHttp.Contracts;

/// <summary>
/// The HTTP header that may be specified in a client request.
/// </summary>
public record RequestHeader(
    string Key,
    string Value);
