namespace EazyHttp.Exceptions;

/// <summary>
/// Represents errors that occur when an http request returns a response code which is not a success.
/// </summary>
public class FailedRequestException : Exception
{
  /// <summary>
  /// Gets the HTTP status code returned by the response.
  /// </summary>
  public HttpStatusCode StatusCode { get; }

  /// <summary>
  /// Gets the HTTP method used for the request.
  /// </summary>
  public HttpMethod Method { get; }

  /// <summary>
  /// Returns the raw response string content
  /// </summary>
  public string ResponseContent { get; }

  /// <summary>
  /// Initializes a new instance of the <see cref="FailedRequestException"/>
  /// </summary>
  public FailedRequestException(
    HttpStatusCode statusCode,
    HttpMethod method) : this(
          statusCode,
          method,
          null)
  { }

  /// <summary>
  /// Initializes a new instance of the <see cref="FailedRequestException"/>
  /// </summary>
  public FailedRequestException(
    HttpStatusCode statusCode,
    HttpMethod method,
    string? message) : this(
          statusCode,
          method,
          message,
          null)
  { }

  /// <summary>
  /// Initializes a new instance of the <see cref="FailedRequestException"/>
  /// </summary>
  public FailedRequestException(
    HttpStatusCode statusCode,
    HttpMethod method,
    string? message,
    string? responseContent) : this(
          statusCode,
          method,
          message,
          responseContent,
          null)
  { }

  /// <summary>
  /// Initializes a new instance of the <see cref="FailedRequestException"/>
  /// </summary>
  public FailedRequestException(
    HttpStatusCode statusCode,
    HttpMethod method,
    string? message,
    string? responseContent,
    Exception? innerException) : base(
          message,
          innerException)
  {
    StatusCode = statusCode;
    Method = method;

    responseContent ??= "";

    ResponseContent = responseContent;
  }
}
