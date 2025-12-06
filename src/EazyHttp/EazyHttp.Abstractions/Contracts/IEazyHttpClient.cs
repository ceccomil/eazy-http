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
  HttpClient HttpClient { get; }

  // -------- GET --------

  Task<TResult?> GetAsync<TResult>(
    string route,
    HttpQuery? query = null,
    IEnumerable<RequestHeader>? additionalHeaders = default,
    CancellationToken cancellationToken = default);

  Task<ResponseEnvelope<TResult?>> GetWithResponseAsync<TResult>(
    string route,
    HttpQuery? query = null,
    IEnumerable<RequestHeader>? additionalHeaders = default,
    CancellationToken cancellationToken = default);

  Task GetAsync(
    string route,
    HttpQuery? query = null,
    IEnumerable<RequestHeader>? additionalHeaders = default,
    CancellationToken cancellationToken = default);

  // -------- PUT --------

  Task<TResult?> PutAsync<TResult>(
    string route,
    object body,
    HttpQuery? query = null,
    IEnumerable<RequestHeader>? additionalHeaders = default,
    CancellationToken cancellationToken = default);

  Task<ResponseEnvelope<TResult?>> PutWithResponseAsync<TResult>(
    string route,
    object body,
    HttpQuery? query = null,
    IEnumerable<RequestHeader>? additionalHeaders = default,
    CancellationToken cancellationToken = default);

  Task PutAsync(
    string route,
    object body,
    HttpQuery? query = null,
    IEnumerable<RequestHeader>? additionalHeaders = default,
    CancellationToken cancellationToken = default);

  // -------- POST --------

  Task<TResult?> PostAsync<TResult>(
    string route,
    object body,
    HttpQuery? query = null,
    IEnumerable<RequestHeader>? additionalHeaders = default,
    CancellationToken cancellationToken = default);

  Task<ResponseEnvelope<TResult?>> PostWithResponseAsync<TResult>(
    string route,
    object body,
    HttpQuery? query = null,
    IEnumerable<RequestHeader>? additionalHeaders = default,
    CancellationToken cancellationToken = default);

  Task PostAsync(
    string route,
    object body,
    HttpQuery? query = null,
    IEnumerable<RequestHeader>? additionalHeaders = default,
    CancellationToken cancellationToken = default);

  // -------- DELETE --------

  Task<TResult?> DeleteAsync<TResult>(
    string route,
    HttpQuery? query = default,
    IEnumerable<RequestHeader>? additionalHeaders = default,
    CancellationToken cancellationToken = default);

  Task<ResponseEnvelope<TResult?>> DeleteWithResponseAsync<TResult>(
    string route,
    HttpQuery? query = default,
    IEnumerable<RequestHeader>? additionalHeaders = default,
    CancellationToken cancellationToken = default);

  Task DeleteAsync(
    string route,
    HttpQuery? query = default,
    IEnumerable<RequestHeader>? additionalHeaders = default,
    CancellationToken cancellationToken = default);

  // -------- PATCH --------

  Task<TResult?> PatchAsync<TResult>(
    string route,
    object body,
    HttpQuery? query = null,
    IEnumerable<RequestHeader>? additionalHeaders = default,
    CancellationToken cancellationToken = default);

  Task<ResponseEnvelope<TResult?>> PatchWithResponseAsync<TResult>(
    string route,
    object body,
    HttpQuery? query = null,
    IEnumerable<RequestHeader>? additionalHeaders = default,
    CancellationToken cancellationToken = default);

  Task PatchAsync(
    string route,
    object body,
    HttpQuery? query = null,
    IEnumerable<RequestHeader>? additionalHeaders = default,
    CancellationToken cancellationToken = default);

  // -------- multipart/form-data --------

  Task<TResult?> PostFormAsync<TResult>(
    string route,
    IEnumerable<FormElement> elements,
    HttpQuery? query = null,
    IEnumerable<RequestHeader>? additionalHeaders = default,
    CancellationToken cancellationToken = default);

  Task<ResponseEnvelope<TResult?>> PostFormWithResponseAsync<TResult>(
    string route,
    IEnumerable<FormElement> elements,
    HttpQuery? query = null,
    IEnumerable<RequestHeader>? additionalHeaders = default,
    CancellationToken cancellationToken = default);

  Task PostFormAsync(
    string route,
    IEnumerable<FormElement> elements,
    HttpQuery? query = null,
    IEnumerable<RequestHeader>? additionalHeaders = default,
    CancellationToken cancellationToken = default);

  // -------- application/x-www-form-urlencoded --------

  Task<TResult?> PostUrlEncodedFormAsync<TResult>(
    string route,
    IEnumerable<KeyValuePair<string, string?>> elements,
    HttpQuery? query = null,
    IEnumerable<RequestHeader>? additionalHeaders = default,
    CancellationToken cancellationToken = default);
  
  Task<ResponseEnvelope<TResult?>> PostUrlEncodedFormWithResponseAsync<TResult>(
    string route,
    IEnumerable<KeyValuePair<string, string?>> elements,
    HttpQuery? query = null,
    IEnumerable<RequestHeader>? additionalHeaders = default,
    CancellationToken cancellationToken = default);

  Task PostUrlEncodedFormAsync(
    string route,
    IEnumerable<KeyValuePair<string, string?>> elements,
    HttpQuery? query = null,
    IEnumerable<RequestHeader>? additionalHeaders = default,
    CancellationToken cancellationToken = default);
}
