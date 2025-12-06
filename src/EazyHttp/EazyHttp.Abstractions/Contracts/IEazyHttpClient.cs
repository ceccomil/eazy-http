namespace EazyHttp.Contracts;

/// <summary>
/// Defines a high-level HTTP client interface for sending requests and receiving responses using common HTTP methods.
/// Provides strongly-typed methods for GET, POST, PUT, PATCH, and DELETE operations, as well as support for multipart
/// and form-encoded requests.
/// </summary>
/// <remarks>IEazyHttpClient abstracts HTTP communication, enabling serialization and deserialization of request
/// and response bodies. It supports adding custom headers, query parameters, and cancellation tokens for request
/// control. The interface is designed for asynchronous usage and integrates with the underlying HttpClient instance,
/// allowing advanced configuration and reuse. Methods returning ResponseEnvelope provide access to HTTP metadata such
/// as status codes and headers alongside the deserialized body.</remarks>
public interface IEazyHttpClient
{
  /// <summary>
  /// Gets the underlying <see cref="HttpClient"/> instance used for requests.
  /// </summary>
  HttpClient HttpClient { get; }

  // -------- GET --------

  /// <summary>
  /// Sends a GET request and deserializes the response body to <typeparamref name="TResult"/>.
  /// </summary>
  /// <param name="route">The request route.</param>
  /// <param name="query">Optional query parameters.</param>
  /// <param name="additionalHeaders">Optional additional headers.</param>
  /// <param name="cancellationToken">Optional cancellation token.</param>
  /// <returns>The deserialized response body.</returns>
  Task<TResult?> GetAsync<TResult>(
    string route,
    HttpQuery? query = null,
    IEnumerable<RequestHeader>? additionalHeaders = default,
    CancellationToken cancellationToken = default);

  /// <summary>
  /// Sends a GET request and returns a <see cref="ResponseEnvelope{TResult}"/> containing HTTP metadata and the deserialized body.
  /// </summary>
  Task<ResponseEnvelope<TResult?>> GetWithResponseAsync<TResult>(
    string route,
    HttpQuery? query = null,
    IEnumerable<RequestHeader>? additionalHeaders = default,
    CancellationToken cancellationToken = default);

  /// <summary>
  /// Sends a GET request without expecting a response body.
  /// </summary>
  Task GetAsync(
    string route,
    HttpQuery? query = null,
    IEnumerable<RequestHeader>? additionalHeaders = default,
    CancellationToken cancellationToken = default);

  // -------- PUT --------

  /// <summary>
  /// Sends a PUT request with a body and deserializes the response body to <typeparamref name="TResult"/>.
  /// </summary>
  Task<TResult?> PutAsync<TResult>(
    string route,
    object body,
    HttpQuery? query = null,
    IEnumerable<RequestHeader>? additionalHeaders = default,
    CancellationToken cancellationToken = default);

  /// <summary>
  /// Sends a PUT request with a body and returns a <see cref="ResponseEnvelope{TResult}"/>.
  /// </summary>
  Task<ResponseEnvelope<TResult?>> PutWithResponseAsync<TResult>(
    string route,
    object body,
    HttpQuery? query = null,
    IEnumerable<RequestHeader>? additionalHeaders = default,
    CancellationToken cancellationToken = default);

  /// <summary>
  /// Sends a PUT request with a body and no response body.
  /// </summary>
  Task PutAsync(
    string route,
    object body,
    HttpQuery? query = null,
    IEnumerable<RequestHeader>? additionalHeaders = default,
    CancellationToken cancellationToken = default);

  // -------- POST --------

  /// <summary>
  /// Sends a POST request with a body and deserializes the response body to <typeparamref name="TResult"/>.
  /// </summary>
  Task<TResult?> PostAsync<TResult>(
    string route,
    object body,
    HttpQuery? query = null,
    IEnumerable<RequestHeader>? additionalHeaders = default,
    CancellationToken cancellationToken = default);

  /// <summary>
  /// Sends a POST request with a body and returns a <see cref="ResponseEnvelope{TResult}"/>.
  /// </summary>
  Task<ResponseEnvelope<TResult?>> PostWithResponseAsync<TResult>(
    string route,
    object body,
    HttpQuery? query = null,
    IEnumerable<RequestHeader>? additionalHeaders = default,
    CancellationToken cancellationToken = default);

  /// <summary>
  /// Sends a POST request with a body and no response body.
  /// </summary>
  Task PostAsync(
    string route,
    object body,
    HttpQuery? query = null,
    IEnumerable<RequestHeader>? additionalHeaders = default,
    CancellationToken cancellationToken = default);

  // -------- DELETE --------

  /// <summary>
  /// Sends a DELETE request and deserializes the response body to <typeparamref name="TResult"/>.
  /// </summary>
  Task<TResult?> DeleteAsync<TResult>(
    string route,
    HttpQuery? query = default,
    IEnumerable<RequestHeader>? additionalHeaders = default,
    CancellationToken cancellationToken = default);

  /// <summary>
  /// Sends a DELETE request and returns a <see cref="ResponseEnvelope{TResult}"/>.
  /// </summary>
  Task<ResponseEnvelope<TResult?>> DeleteWithResponseAsync<TResult>(
    string route,
    HttpQuery? query = default,
    IEnumerable<RequestHeader>? additionalHeaders = default,
    CancellationToken cancellationToken = default);

  /// <summary>
  /// Sends a DELETE request without expecting a response body.
  /// </summary>
  Task DeleteAsync(
    string route,
    HttpQuery? query = default,
    IEnumerable<RequestHeader>? additionalHeaders = default,
    CancellationToken cancellationToken = default);

  // -------- PATCH --------

  /// <summary>
  /// Sends a PATCH request with a body and deserializes the response body to <typeparamref name="TResult"/>.
  /// </summary>
  Task<TResult?> PatchAsync<TResult>(
    string route,
    object body,
    HttpQuery? query = null,
    IEnumerable<RequestHeader>? additionalHeaders = default,
    CancellationToken cancellationToken = default);

  /// <summary>
  /// Sends a PATCH request with a body and returns a <see cref="ResponseEnvelope{TResult}"/>.
  /// </summary>
  Task<ResponseEnvelope<TResult?>> PatchWithResponseAsync<TResult>(
    string route,
    object body,
    HttpQuery? query = null,
    IEnumerable<RequestHeader>? additionalHeaders = default,
    CancellationToken cancellationToken = default);

  /// <summary>
  /// Sends a PATCH request with a body and no response body.
  /// </summary>
  Task PatchAsync(
    string route,
    object body,
    HttpQuery? query = null,
    IEnumerable<RequestHeader>? additionalHeaders = default,
    CancellationToken cancellationToken = default);

  // -------- multipart/form-data --------

  /// <summary>
  /// Sends a multipart/form-data POST request and deserializes the response body to <typeparamref name="TResult"/>.
  /// </summary>
  Task<TResult?> PostFormAsync<TResult>(
    string route,
    IEnumerable<FormElement> elements,
    HttpQuery? query = null,
    IEnumerable<RequestHeader>? additionalHeaders = default,
    CancellationToken cancellationToken = default);

  /// <summary>
  /// Sends a multipart/form-data POST request and returns a <see cref="ResponseEnvelope{TResult}"/>.
  /// </summary>
  Task<ResponseEnvelope<TResult?>> PostFormWithResponseAsync<TResult>(
    string route,
    IEnumerable<FormElement> elements,
    HttpQuery? query = null,
    IEnumerable<RequestHeader>? additionalHeaders = default,
    CancellationToken cancellationToken = default);

  /// <summary>
  /// Sends a multipart/form-data POST request without expecting a response body.
  /// </summary>
  Task PostFormAsync(
    string route,
    IEnumerable<FormElement> elements,
    HttpQuery? query = null,
    IEnumerable<RequestHeader>? additionalHeaders = default,
    CancellationToken cancellationToken = default);

  // -------- application/x-www-form-urlencoded --------

  /// <summary>
  /// Sends an application/x-www-form-urlencoded POST request and deserializes the response body to <typeparamref name="TResult"/>.
  /// </summary>
  Task<TResult?> PostUrlEncodedFormAsync<TResult>(
    string route,
    IEnumerable<KeyValuePair<string, string?>> elements,
    HttpQuery? query = null,
    IEnumerable<RequestHeader>? additionalHeaders = default,
    CancellationToken cancellationToken = default);
  
  /// <summary>
  /// Sends an application/x-www-form-urlencoded POST request and returns a <see cref="ResponseEnvelope{TResult}"/>.
  /// </summary>
  Task<ResponseEnvelope<TResult?>> PostUrlEncodedFormWithResponseAsync<TResult>(
    string route,
    IEnumerable<KeyValuePair<string, string?>> elements,
    HttpQuery? query = null,
    IEnumerable<RequestHeader>? additionalHeaders = default,
    CancellationToken cancellationToken = default);

  /// <summary>
  /// Sends an application/x-www-form-urlencoded POST request without expecting a response body.
  /// </summary>
  Task PostUrlEncodedFormAsync(
    string route,
    IEnumerable<KeyValuePair<string, string?>> elements,
    HttpQuery? query = null,
    IEnumerable<RequestHeader>? additionalHeaders = default,
    CancellationToken cancellationToken = default);
}
