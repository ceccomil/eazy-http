namespace EazyHttp.Contracts;

/// <summary>
/// Defines a simplified HTTP client interface for sending requests and receiving responses using common HTTP methods.
/// This interface does not support per-request custom headers and is intended for scenarios where configuration is
/// applied at the client level (e.g., via named clients and persistent headers).
/// </summary>
/// <remarks>
/// <para>
/// <see cref="ISimpleEazyHttpClient"/> provides strongly-typed methods for GET, POST, PUT, PATCH and DELETE operations,
/// as well as support for multipart and form-encoded POST requests.
/// </para>
/// <para>
/// Methods returning <see cref="ResponseEnvelope{TResult}"/> expose HTTP metadata such as status codes and headers
/// alongside the deserialized body. Methods returning only <c>TResult</c> are convenience overloads for the common
/// case where callers only care about the response body.
/// </para>
/// </remarks>
public interface ISimpleEazyHttpClient
{
  HttpClient HttpClient { get; }

  // -------- GET --------

  Task<TResult?> GetAsync<TResult>(
    string route,
    HttpQuery? query = null,
    CancellationToken cancellationToken = default);

  Task<ResponseEnvelope<TResult?>> GetWithResponseAsync<TResult>(
    string route,
    HttpQuery? query = null,
    CancellationToken cancellationToken = default);

  Task GetAsync(
    string route,
    HttpQuery? query = null,
    CancellationToken cancellationToken = default);

  // -------- PUT --------

  Task<TResult?> PutAsync<TResult>(
    string route,
    object body,
    HttpQuery? query = null,
    CancellationToken cancellationToken = default);

  Task<ResponseEnvelope<TResult?>> PutWithResponseAsync<TResult>(
    string route,
    object body,
    HttpQuery? query = null,
    CancellationToken cancellationToken = default);

  Task PutAsync(
    string route,
    object body,
    HttpQuery? query = null,
    CancellationToken cancellationToken = default);

  // -------- POST --------

  Task<TResult?> PostAsync<TResult>(
    string route,
    object body,
    HttpQuery? query = null,
    CancellationToken cancellationToken = default);

  Task<ResponseEnvelope<TResult?>> PostWithResponseAsync<TResult>(
    string route,
    object body,
    HttpQuery? query = null,
    CancellationToken cancellationToken = default);

  Task PostAsync(
    string route,
    object body,
    HttpQuery? query = null,
    CancellationToken cancellationToken = default);

  // -------- DELETE --------

  Task<TResult?> DeleteAsync<TResult>(
    string route,
    HttpQuery? query = null,
    CancellationToken cancellationToken = default);

  Task<ResponseEnvelope<TResult?>> DeleteWithResponseAsync<TResult>(
    string route,
    HttpQuery? query = null,
    CancellationToken cancellationToken = default);

  Task DeleteAsync(
    string route,
    HttpQuery? query = null,
    CancellationToken cancellationToken = default);

  // -------- PATCH --------

  Task<TResult?> PatchAsync<TResult>(
    string route,
    object body,
    HttpQuery? query = null,
    CancellationToken cancellationToken = default);

  Task<ResponseEnvelope<TResult?>> PatchWithResponseAsync<TResult>(
    string route,
    object body,
    HttpQuery? query = null,
    CancellationToken cancellationToken = default);

  Task PatchAsync(
    string route,
    object body,
    HttpQuery? query = null,
    CancellationToken cancellationToken = default);

  // -------- multipart/form-data --------

  Task<TResult?> PostFormAsync<TResult>(
    string route,
    IEnumerable<FormElement> elements,
    HttpQuery? query = null,
    CancellationToken cancellationToken = default);

  Task<ResponseEnvelope<TResult?>> PostFormWithResponseAsync<TResult>(
    string route,
    IEnumerable<FormElement> elements,
    HttpQuery? query = null,
    CancellationToken cancellationToken = default);

  Task PostFormAsync(
    string route,
    IEnumerable<FormElement> elements,
    HttpQuery? query = null,
    CancellationToken cancellationToken = default);

  // -------- application/x-www-form-urlencoded --------

  Task<TResult?> PostUrlEncodedFormAsync<TResult>(
    string route,
    IEnumerable<KeyValuePair<string, string?>> elements,
    HttpQuery? query = null,
    CancellationToken cancellationToken = default);

  Task<ResponseEnvelope<TResult?>> PostUrlEncodedFormWithResponseAsync<TResult>(
    string route,
    IEnumerable<KeyValuePair<string, string?>> elements,
    HttpQuery? query = null,
    CancellationToken cancellationToken = default);

  Task PostUrlEncodedFormAsync(
    string route,
    IEnumerable<KeyValuePair<string, string?>> elements,
    HttpQuery? query = null,
    CancellationToken cancellationToken = default);
}
