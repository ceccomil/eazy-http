namespace EazyHttp.Contracts;

/// <summary>
/// Encapsulates an HTTP response, including the deserialized response body, status code, and associated headers.
/// </summary>
/// <remarks>Use this type to access both the HTTP response metadata and the strongly typed response body in a
/// single object. This is commonly used in HTTP client libraries to provide a unified result for API calls, allowing
/// callers to inspect status codes, headers, and content together.</remarks>
/// <typeparam name="TResult">The type of the deserialized response body contained in the envelope.</typeparam>
public sealed record ResponseEnvelope<TResult>
{
  /// <summary>
  /// Gets the deserialized body of the response, if available.
  /// </summary>
  public TResult? Body { get; }

  /// <summary>
  /// Gets the raw body content as a byte array.
  /// </summary>
  public byte[] RawBody { get; }

  /// <summary>
  /// Gets the HTTP status code returned by the response.
  /// </summary>
  public HttpStatusCode StatusCode { get; }

  /// <summary>
  /// Gets the collection of HTTP response headers included with the response.
  /// </summary>
  /// <remarks>The headers provide metadata about the HTTP response, such as content type, caching directives,
  /// and server information. Modifying the headers may affect how clients interpret the response.</remarks>
  public HttpResponseHeaders Headers { get; }

  /// <summary>
  /// Gets the HTTP content headers associated with the content.
  /// </summary>
  /// <remarks>Use this property to access headers such as Content-Type, Content-Length, and others that
  /// describe the content being sent or received. The returned headers are specific to the content and do not include
  /// general HTTP headers.</remarks>
  public HttpContentHeaders ContentHeaders { get; }

  /// <summary>
  /// Gets a value indicating whether the HTTP response status code represents a successful operation.
  /// </summary>
  /// <remarks>A status code is considered successful if it is in the range 200 to 299, inclusive. This property
  /// is commonly used to determine if an HTTP request completed successfully before processing the response
  /// content.</remarks>
  public bool IsSuccessStatusCode { get; }

  /// <summary>
  /// Gets or sets the error message generated during deserialization, if an error occurred.
  /// </summary>
  /// <remarks>This property is typically set when a deserialization operation fails. If deserialization
  /// succeeds, the value is <see langword="null"/>.</remarks>
  public string? DeserializationErrorMessage { get; set; }

  /// <summary>
  /// Initializes a new instance of the ResponseEnvelope class with the specified response body, raw content, status
  /// code, and HTTP headers.
  /// </summary>
  /// <param name="body">The deserialized response body of type TResult, or null if the response does not contain a body.</param>
  /// <param name="rawBody">The raw response content as a byte array. If null, an empty array is used.</param>
  /// <param name="statusCode">The HTTP status code returned by the server.</param>
  /// <param name="headers">The collection of HTTP response headers associated with the response.</param>
  /// <param name="contentHeaders">The collection of HTTP content headers associated with the response body.</param>
  /// <param name="isSuccessStatusCode">Indicates whether the HTTP response status code represents a successful response. <see langword="true"/> if the
  /// status code is in the successful range; otherwise, <see langword="false"/>.</param>
  public ResponseEnvelope(
    TResult? body,
    byte[] rawBody,
    HttpStatusCode statusCode,
    HttpResponseHeaders headers,
    HttpContentHeaders contentHeaders,
    bool isSuccessStatusCode)
  {
    Body = body;
    RawBody = rawBody ?? [];
    StatusCode = statusCode;
    Headers = headers;
    ContentHeaders = contentHeaders;
    IsSuccessStatusCode = isSuccessStatusCode;
  }
}