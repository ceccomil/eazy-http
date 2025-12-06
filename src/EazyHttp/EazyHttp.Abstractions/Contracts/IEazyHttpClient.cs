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
  /// Gets the underlying <see cref="HttpClient"/> used to send requests.
  /// </summary>
  HttpClient HttpClient { get; }

  // -------- GET --------

  /// <summary>
  /// Sends an asynchronous HTTP GET request to the specified route and returns the response deserialized to the
  /// specified type.
  /// </summary>
  /// <remarks>The method will throw an exception if the request fails due to network errors or if the response
  /// cannot be deserialized to the specified type. The caller is responsible for handling cancellation and exceptions
  /// as appropriate.</remarks>
  /// <typeparam name="TResult">The type to which the HTTP response content will be deserialized. Must be compatible with the response format.</typeparam>
  /// <param name="route">The relative or absolute URI route to which the GET request is sent. Cannot be null or empty.</param>
  /// <param name="query">An optional query string to append to the request URI. If null, no query parameters are added.</param>
  /// <param name="additionalHeaders">An optional collection of additional HTTP headers to include in the request. If null, no extra headers are sent.</param>
  /// <param name="cancellationToken">A token that can be used to cancel the asynchronous operation.</param>
  /// <returns>A task that represents the asynchronous operation. The task result contains the deserialized response of type
  /// <typeparamref name="TResult"/> if the request is successful; otherwise, <see langword="null"/>.</returns>
  Task<TResult?> GetAsync<TResult>(
    string route,
    HttpQuery? query = default,
    IEnumerable<RequestHeader>? additionalHeaders = default,
    CancellationToken cancellationToken = default);

  /// <summary>
  /// Sends an asynchronous HTTP GET request to the specified route and returns the response envelope containing the
  /// deserialized result.
  /// </summary>
  /// <remarks>The method does not throw an exception for non-success HTTP status codes; instead, the status and
  /// any error information are included in the returned response envelope. This method is thread-safe and can be called
  /// concurrently.</remarks>
  /// <typeparam name="TResult">The type to which the response content will be deserialized. Must be compatible with the response payload.</typeparam>
  /// <param name="route">The relative route or endpoint to which the GET request will be sent. Cannot be null or empty.</param>
  /// <param name="query">An optional query object representing URL parameters to include in the request. If null, no query parameters are
  /// added.</param>
  /// <param name="additionalHeaders">An optional collection of additional HTTP headers to include in the request. If null, no extra headers are sent.</param>
  /// <param name="cancellationToken">A cancellation token that can be used to cancel the request.</param>
  /// <returns>A task that represents the asynchronous operation. The task result contains a response envelope with the
  /// deserialized result of type <typeparamref name="TResult"/>. The result may be null if the response contains no
  /// content.</returns>
  Task<ResponseEnvelope<TResult?>> GetWithResponseAsync<TResult>(
    string route,
    HttpQuery? query = default,
    IEnumerable<RequestHeader>? additionalHeaders = default,
    CancellationToken cancellationToken = default);

  /// <summary>
  /// Sends an asynchronous HTTP GET request to the specified route with optional query parameters and headers.
  /// </summary>
  /// <param name="route">The relative or absolute route to which the GET request will be sent. Cannot be null or empty.</param>
  /// <param name="query">An optional set of query parameters to include in the request. If null, no query parameters are added.</param>
  /// <param name="additionalHeaders">An optional collection of additional headers to include in the request. If null, no extra headers are sent.</param>
  /// <param name="cancellationToken">A token that can be used to cancel the asynchronous operation.</param>
  /// <returns>A task that represents the asynchronous GET operation.</returns>
  Task GetAsync(
    string route,
    HttpQuery? query = default,
    IEnumerable<RequestHeader>? additionalHeaders = default,
    CancellationToken cancellationToken = default);

  // -------- PUT --------

  /// <summary>
  /// Sends an asynchronous HTTP PUT request to the specified route with the provided request body and optional headers,
  /// and returns the deserialized response.
  /// </summary>
  /// <remarks>The request body is serialized according to the configured content type. If the server response
  /// cannot be deserialized to <typeparamref name="TResult"/>, the result will be <see langword="null"/>. This method
  /// does not throw for non-success HTTP status codes; callers should check the result for null to detect
  /// errors.</remarks>
  /// <typeparam name="TResult">The type to which the response content will be deserialized. Must be compatible with the response format.</typeparam>
  /// <param name="route">The relative or absolute URI of the endpoint to which the PUT request is sent. Cannot be null or empty.</param>
  /// <param name="body">The object representing the request payload to be serialized and sent in the PUT request body. Cannot be null.</param>
  /// <param name="additionalHeaders">An optional collection of additional headers to include in the request. If null, no extra headers are added.</param>
  /// <param name="cancellationToken">A token to monitor for cancellation requests. The operation is canceled if the token is triggered.</param>
  /// <returns>A task that represents the asynchronous operation. The task result contains the deserialized response of type
  /// <typeparamref name="TResult"/> if the request is successful; otherwise, <see langword="null"/>.</returns>
  Task<TResult?> PutAsync<TResult>(
    string route,
    object body,
    IEnumerable<RequestHeader>? additionalHeaders = default,
    CancellationToken cancellationToken = default);

  /// <summary>
  /// Sends an HTTP PUT request to the specified route with the provided request body and returns a response envelope
  /// containing the deserialized result.
  /// </summary>
  /// <typeparam name="TResult">The type to which the response content will be deserialized.</typeparam>
  /// <param name="route">The relative URI route to which the PUT request is sent. Cannot be null or empty.</param>
  /// <param name="body">The object representing the request payload to be serialized and sent in the PUT request. Cannot be null.</param>
  /// <param name="additionalHeaders">An optional collection of additional headers to include in the request. If null, no extra headers are added.</param>
  /// <param name="cancellationToken">A cancellation token that can be used to cancel the request.</param>
  /// <returns>A task that represents the asynchronous operation. The task result contains a response envelope with the
  /// deserialized result of type <typeparamref name="TResult"/>. The result may be null if the response contains no
  /// content.</returns>
  Task<ResponseEnvelope<TResult?>> PutWithResponseAsync<TResult>(
    string route,
    object body,
    IEnumerable<RequestHeader>? additionalHeaders = default,
    CancellationToken cancellationToken = default);

  /// <summary>
  /// Sends an asynchronous HTTP PUT request to the specified route with the provided request body and optional headers.
  /// </summary>
  /// <remarks>The request body is serialized according to the implementation's configuration. The method does
  /// not throw on non-success HTTP status codes; callers should inspect the response as appropriate.</remarks>
  /// <param name="route">The relative or absolute URI of the endpoint to which the PUT request is sent. Cannot be null or empty.</param>
  /// <param name="body">The object to serialize and include as the request body. Cannot be null.</param>
  /// <param name="additionalHeaders">An optional collection of additional headers to include in the request. If null or omitted, no extra headers are
  /// added.</param>
  /// <param name="cancellationToken">A token that can be used to cancel the operation. The default value is <see cref="CancellationToken.None"/>.</param>
  /// <returns>A task that represents the asynchronous operation of sending the PUT request.</returns>
  Task PutAsync(
    string route,
    object body,
    IEnumerable<RequestHeader>? additionalHeaders = default,
    CancellationToken cancellationToken = default);

  // -------- POST --------

  /// <summary>
  /// Sends an HTTP POST request to the specified route with the provided request body and optional headers, and
  /// asynchronously returns the deserialized response.
  /// </summary>
  /// <remarks>The request body is serialized according to the configured content type (e.g., JSON). If the
  /// response cannot be deserialized to <typeparamref name="TResult"/>, the result will be <see langword="null"/>. This
  /// method does not throw for non-success HTTP status codes; callers should check for <see langword="null"/> or handle
  /// error responses as appropriate.</remarks>
  /// <typeparam name="TResult">The type to which the response content will be deserialized. Must be compatible with the response format.</typeparam>
  /// <param name="route">The relative or absolute URI of the endpoint to which the POST request is sent. Cannot be null or empty.</param>
  /// <param name="body">The object representing the request payload to be serialized and sent in the POST request body. Cannot be null.</param>
  /// <param name="additionalHeaders">An optional collection of additional headers to include in the request. If null, no extra headers are added.</param>
  /// <param name="cancellationToken">A token to monitor for cancellation requests. The operation is canceled if the token is triggered.</param>
  /// <returns>A task that represents the asynchronous operation. The task result contains the deserialized response of type
  /// <typeparamref name="TResult"/> if the request is successful; otherwise, <see langword="null"/>.</returns>
  Task<TResult?> PostAsync<TResult>(
    string route,
    object body,
    IEnumerable<RequestHeader>? additionalHeaders = default,
    CancellationToken cancellationToken = default);

  /// <summary>
  /// Sends an HTTP POST request to the specified route with the provided request body and returns the deserialized
  /// response envelope containing the result.
  /// </summary>
  /// <remarks>The method serializes the request body and sends it as JSON. The response is deserialized into
  /// the specified type parameter. If the request fails, the response envelope may contain error information. This
  /// method does not throw exceptions for HTTP error responses; instead, error details are included in the
  /// ResponseEnvelope.</remarks>
  /// <typeparam name="TResult">The type to which the response content will be deserialized.</typeparam>
  /// <param name="route">The relative route or endpoint to which the POST request is sent. Cannot be null or empty.</param>
  /// <param name="body">The object representing the request payload to be serialized and sent in the POST request body. Cannot be null.</param>
  /// <param name="additionalHeaders">An optional collection of additional headers to include in the request. If null or empty, no extra headers are
  /// added.</param>
  /// <param name="cancellationToken">A cancellation token that can be used to cancel the request.</param>
  /// <returns>A task that represents the asynchronous operation. The task result contains a ResponseEnvelope holding the
  /// deserialized result of type TResult, or null if the response has no content.</returns>
  Task<ResponseEnvelope<TResult?>> PostWithResponseAsync<TResult>(
    string route,
    object body,
    IEnumerable<RequestHeader>? additionalHeaders = default,
    CancellationToken cancellationToken = default);

  /// <summary>
  /// Sends an asynchronous HTTP POST request to the specified route with the provided request body and optional
  /// headers.
  /// </summary>
  /// <param name="route">The relative route or endpoint to which the POST request is sent. Cannot be null or empty.</param>
  /// <param name="body">The object representing the request payload to be serialized and sent in the body of the POST request. Cannot be
  /// null.</param>
  /// <param name="additionalHeaders">An optional collection of additional headers to include in the request. If null or omitted, no extra headers are
  /// added.</param>
  /// <param name="cancellationToken">A token that can be used to cancel the operation. The default value is <see cref="CancellationToken.None"/>.</param>
  /// <returns>A task that represents the asynchronous operation of sending the POST request.</returns>
  Task PostAsync(
    string route,
    object body,
    IEnumerable<RequestHeader>? additionalHeaders = default,
    CancellationToken cancellationToken = default);

  // -------- DELETE --------

  /// <summary>
  /// Sends an asynchronous HTTP DELETE request to the specified route and returns the response deserialized to the
  /// specified type.
  /// </summary>
  /// <remarks>If the response does not contain content or cannot be deserialized to <typeparamref
  /// name="TResult"/>, the result will be <see langword="null"/>. The method does not throw for non-success HTTP status
  /// codes; callers should check the result for null to determine if the operation succeeded.</remarks>
  /// <typeparam name="TResult">The type to which the response content will be deserialized. Must be compatible with the response format.</typeparam>
  /// <param name="route">The relative or absolute URI route to which the DELETE request is sent. Cannot be null or empty.</param>
  /// <param name="query">An optional query string to append to the request URI. If null, no query parameters are added.</param>
  /// <param name="additionalHeaders">An optional collection of additional headers to include in the request. If null, no extra headers are sent.</param>
  /// <param name="cancellationToken">A cancellation token that can be used to cancel the request.</param>
  /// <returns>A task that represents the asynchronous operation. The task result contains the deserialized response of type
  /// <typeparamref name="TResult"/> if the request is successful; otherwise, <see langword="null"/>.</returns>
  Task<TResult?> DeleteAsync<TResult>(
    string route,
    HttpQuery? query = default,
    IEnumerable<RequestHeader>? additionalHeaders = default,
    CancellationToken cancellationToken = default);

  /// <summary>
  /// Sends an HTTP DELETE request to the specified route and returns the server's response, including any deserialized
  /// result data.
  /// </summary>
  /// <remarks>The method does not throw if the server returns an error response; instead, error details are
  /// included in the returned response envelope. The caller is responsible for interpreting the response status and
  /// result.</remarks>
  /// <typeparam name="TResult">The type of the result expected from the server's response. Must be compatible with the response payload, or
  /// nullable if no content is expected.</typeparam>
  /// <param name="route">The relative route or endpoint to which the DELETE request is sent. Cannot be null or empty.</param>
  /// <param name="query">An optional set of query parameters to include in the request URL. If null, no query parameters are added.</param>
  /// <param name="additionalHeaders">An optional collection of additional HTTP headers to include with the request. If null, no extra headers are sent.</param>
  /// <param name="cancellationToken">A cancellation token that can be used to cancel the request operation.</param>
  /// <returns>A task that represents the asynchronous operation. The result contains a response envelope with the server's
  /// response and the deserialized result of type <typeparamref name="TResult"/> if available; otherwise, null.</returns>
  Task<ResponseEnvelope<TResult?>> DeleteWithResponseAsync<TResult>(
    string route,
    HttpQuery? query = default,
    IEnumerable<RequestHeader>? additionalHeaders = default,
    CancellationToken cancellationToken = default);

  /// <summary>
  /// Sends an asynchronous HTTP DELETE request to the specified route.
  /// </summary>
  /// <remarks>The request is sent using the configured HTTP client. If the operation is canceled via the
  /// cancellation token, the returned task will be canceled.</remarks>
  /// <param name="route">The relative URI route to which the DELETE request will be sent. Cannot be null or empty.</param>
  /// <param name="query">An optional query string to append to the route. If null, no query parameters are included.</param>
  /// <param name="additionalHeaders">An optional collection of additional headers to include in the request. If null, no extra headers are added.</param>
  /// <param name="cancellationToken">A cancellation token that can be used to cancel the operation.</param>
  /// <returns>A task that represents the asynchronous delete operation.</returns>
  Task DeleteAsync(
    string route,
    HttpQuery? query = default,
    IEnumerable<RequestHeader>? additionalHeaders = default,
    CancellationToken cancellationToken = default);

  // -------- PATCH --------

  /// <summary>
  /// Sends an asynchronous HTTP PATCH request to the specified route with the provided request body and optional
  /// headers, and returns the deserialized response.
  /// </summary>
  /// <remarks>The request body is serialized before being sent. If the response cannot be deserialized to
  /// <typeparamref name="TResult"/>, the result will be <see langword="null"/>. This method does not throw for
  /// non-success HTTP status codes; callers should check for null results or handle errors as appropriate.</remarks>
  /// <typeparam name="TResult">The type to which the response content will be deserialized. Must be compatible with the response format.</typeparam>
  /// <param name="route">The relative route or endpoint to which the PATCH request is sent. Cannot be null or empty.</param>
  /// <param name="body">The object representing the request payload to be serialized and sent in the PATCH request body. Cannot be null.</param>
  /// <param name="additionalHeaders">An optional collection of additional headers to include in the request. If null, no extra headers are added.</param>
  /// <param name="cancellationToken">A cancellation token that can be used to cancel the request.</param>
  /// <returns>A task that represents the asynchronous operation. The task result contains the deserialized response of type
  /// <typeparamref name="TResult"/> if the request is successful; otherwise, <see langword="null"/>.</returns>
  Task<TResult?> PatchAsync<TResult>(
    string route,
    object body,
    IEnumerable<RequestHeader>? additionalHeaders = default,
    CancellationToken cancellationToken = default);

  /// <summary>
  /// Sends an HTTP PATCH request to the specified route with the provided request body and returns the deserialized
  /// response envelope asynchronously.
  /// </summary>
  /// <remarks>The method serializes the request body and sends it as a PATCH request. The response is
  /// deserialized into the specified type parameter. This method does not throw if the response is empty; instead, the
  /// result will be null. Thread-safe for concurrent use.</remarks>
  /// <typeparam name="TResult">The type to which the response content will be deserialized. Must be compatible with the response payload.</typeparam>
  /// <param name="route">The relative URI route of the endpoint to which the PATCH request is sent. Cannot be null or empty.</param>
  /// <param name="body">The object representing the request payload to be serialized and sent in the PATCH request body. Cannot be null.</param>
  /// <param name="additionalHeaders">An optional collection of additional headers to include in the request. If null or omitted, no extra headers are
  /// added.</param>
  /// <param name="cancellationToken">A cancellation token that can be used to cancel the request operation.</param>
  /// <returns>A task that represents the asynchronous operation. The task result contains a response envelope with the
  /// deserialized response of type <typeparamref name="TResult"/>. The result may be null if the response contains no
  /// content.</returns>
  Task<ResponseEnvelope<TResult?>> PatchWithResponseAsync<TResult>(
    string route,
    object body,
    IEnumerable<RequestHeader>? additionalHeaders = default,
    CancellationToken cancellationToken = default);

  /// <summary>
  /// Sends an asynchronous HTTP PATCH request to the specified route with the provided request body and optional
  /// headers.
  /// </summary>
  /// <remarks>The request body is serialized before being sent. The method does not return a value; it
  /// completes when the PATCH operation finishes. If the cancellation token is triggered before completion, the
  /// operation is canceled.</remarks>
  /// <param name="route">The relative or absolute URI of the endpoint to which the PATCH request is sent. Cannot be null or empty.</param>
  /// <param name="body">The object representing the content to be serialized and included in the PATCH request body. Cannot be null.</param>
  /// <param name="additionalHeaders">An optional collection of additional headers to include in the request. If null, no extra headers are added.</param>
  /// <param name="cancellationToken">A token that can be used to cancel the PATCH request operation.</param>
  /// <returns>A task that represents the asynchronous PATCH operation.</returns>
  Task PatchAsync(
    string route,
    object body,
    IEnumerable<RequestHeader>? additionalHeaders = default,
    CancellationToken cancellationToken = default);

  // -------- multipart/form-data --------

  /// <summary>
  /// Sends an HTTP POST request with multipart/form-data content to the specified route and returns the deserialized
  /// response.
  /// </summary>
  /// <remarks>The method serializes the provided form elements as multipart/form-data and deserializes the
  /// response content to the specified type. If the response cannot be deserialized, the result will be <see
  /// langword="null"/>. This method does not throw for non-success HTTP status codes; callers should check the result
  /// for null to detect errors.</remarks>
  /// <typeparam name="TResult">The type to which the response content will be deserialized.</typeparam>
  /// <param name="route">The relative or absolute URI of the endpoint to which the form data will be posted. Cannot be null or empty.</param>
  /// <param name="elements">A collection of form elements to include in the multipart/form-data request body. Each element represents a field
  /// or file to be sent.</param>
  /// <param name="additionalHeaders">An optional collection of additional HTTP headers to include in the request. If null, no extra headers are added.</param>
  /// <param name="cancellationToken">A token that can be used to cancel the request operation.</param>
  /// <returns>A task that represents the asynchronous operation. The task result contains the deserialized response of type
  /// <typeparamref name="TResult"/> if the request is successful; otherwise, <see langword="null"/>.</returns>
  Task<TResult?> PostFormAsync<TResult>(
    string route,
    IEnumerable<FormElement> elements,
    IEnumerable<RequestHeader>? additionalHeaders = default,
    CancellationToken cancellationToken = default);

  /// <summary>
  /// Sends a POST request with form data to the specified route and returns the server's response wrapped in a response
  /// envelope.
  /// </summary>
  /// <remarks>The method serializes the provided form elements and sends them as form data. The response
  /// envelope includes both the result and metadata about the response, such as status and error details. This method
  /// does not throw exceptions for HTTP error responses; instead, error information is included in the response
  /// envelope.</remarks>
  /// <typeparam name="TResult">The type of the expected result contained in the response envelope.</typeparam>
  /// <param name="route">The relative route to which the form data will be posted. Cannot be null or empty.</param>
  /// <param name="elements">A collection of form elements representing the data to include in the POST request. Cannot be null.</param>
  /// <param name="additionalHeaders">An optional collection of additional headers to include in the request. If null, no extra headers are added.</param>
  /// <param name="cancellationToken">A cancellation token that can be used to cancel the request.</param>
  /// <returns>A task that represents the asynchronous operation. The task result contains a response envelope with the
  /// deserialized result of type <typeparamref name="TResult"/> if the request is successful; otherwise, the envelope
  /// may contain error information.</returns>
  Task<ResponseEnvelope<TResult?>> PostFormWithResponseAsync<TResult>(
    string route,
    IEnumerable<FormElement> elements,
    IEnumerable<RequestHeader>? additionalHeaders = default,
    CancellationToken cancellationToken = default);

  /// <summary>
  /// Sends an HTTP POST request to the specified route with form data elements, optionally including additional
  /// headers.
  /// </summary>
  /// <remarks>This method sends form data using the "application/x-www-form-urlencoded" content type. If the
  /// operation is canceled via the cancellation token, the returned task will be canceled.</remarks>
  /// <param name="route">The relative or absolute URI route to which the form data will be posted. Cannot be null or empty.</param>
  /// <param name="elements">A collection of form elements to include in the POST request body. Cannot be null; must contain at least one
  /// element.</param>
  /// <param name="additionalHeaders">An optional collection of additional HTTP headers to include in the request. If null or omitted, no extra headers
  /// are added.</param>
  /// <param name="cancellationToken">A cancellation token that can be used to cancel the operation. The default value is <see
  /// cref="CancellationToken.None"/>.</param>
  /// <returns>A task that represents the asynchronous operation of posting the form data. The task completes when the request
  /// has been sent and the response is received.</returns>
  Task PostFormAsync(
    string route,
    IEnumerable<FormElement> elements,
    IEnumerable<RequestHeader>? additionalHeaders = default,
    CancellationToken cancellationToken = default);

  // -------- application/x-www-form-urlencoded --------

  /// <summary>
  /// Sends an HTTP POST request with application/x-www-form-urlencoded data to the specified route and returns the
  /// deserialized response.
  /// </summary>
  /// <remarks>The request is sent with the Content-Type header set to application/x-www-form-urlencoded. The
  /// method will deserialize the response content to the specified type using the configured deserializer. If the
  /// response is unsuccessful or the content is empty, the result may be null.</remarks>
  /// <typeparam name="TResult">The type to which the response content will be deserialized.</typeparam>
  /// <param name="route">The relative or absolute URI of the endpoint to which the form data will be posted. Cannot be null or empty.</param>
  /// <param name="elements">A collection of key-value pairs representing the form fields and their values to include in the request body. Keys
  /// cannot be null; values may be null to indicate an empty field.</param>
  /// <param name="additionalHeaders">An optional collection of additional headers to include in the request. If null, no extra headers are added.</param>
  /// <param name="cancellationToken">A cancellation token that can be used to cancel the request.</param>
  /// <returns>A task that represents the asynchronous operation. The task result contains the deserialized response of type
  /// TResult, or null if the response content is empty.</returns>
  Task<TResult?> PostUrlEncodedFormAsync<TResult>(
    string route,
    IEnumerable<KeyValuePair<string, string?>> elements,
    IEnumerable<RequestHeader>? additionalHeaders = default,
    CancellationToken cancellationToken = default);
  
  /// <summary>
  /// Sends an HTTP POST request with URL-encoded form data to the specified route and asynchronously returns the
  /// deserialized response envelope.
  /// </summary>
  /// <remarks>The request is sent using the application/x-www-form-urlencoded content type. This method does
  /// not throw exceptions for non-success HTTP status codes; instead, status and error information are included in the
  /// returned <see cref="ResponseEnvelope{TResult}"/>.</remarks>
  /// <typeparam name="TResult">The type to which the response content will be deserialized.</typeparam>
  /// <param name="route">The relative route or endpoint to which the POST request is sent. Cannot be null or empty.</param>
  /// <param name="elements">A collection of key-value pairs representing the form data to be URL-encoded and included in the request body.
  /// Keys cannot be null.</param>
  /// <param name="additionalHeaders">An optional collection of additional headers to include in the request. If null or omitted, no extra headers are
  /// added.</param>
  /// <param name="cancellationToken">A cancellation token that can be used to cancel the request operation.</param>
  /// <returns>A task that represents the asynchronous operation. The task result contains a <see
  /// cref="ResponseEnvelope{TResult}"/> with the deserialized response data. The <c>TResult</c> value may be <see
  /// langword="null"/> if the response is empty or cannot be deserialized.</returns>
  Task<ResponseEnvelope<TResult?>> PostUrlEncodedFormWithResponseAsync<TResult>(
    string route,
    IEnumerable<KeyValuePair<string, string?>> elements,
    IEnumerable<RequestHeader>? additionalHeaders = default,
    CancellationToken cancellationToken = default);

  /// <summary>
  /// Sends an HTTP POST request with form data encoded as application/x-www-form-urlencoded to the specified route
  /// asynchronously.
  /// </summary>
  /// <remarks>The request content is sent using the application/x-www-form-urlencoded media type. This method
  /// does not return the response content; use other methods if you need to process the response body.</remarks>
  /// <param name="route">The relative or absolute URI of the endpoint to which the form data will be posted. Cannot be null or empty.</param>
  /// <param name="elements">A collection of key-value pairs representing the form fields and their values to include in the request body. Keys
  /// cannot be null; values may be null to indicate an empty field.</param>
  /// <param name="additionalHeaders">An optional collection of additional HTTP headers to include in the request. If null or omitted, no extra headers
  /// are added.</param>
  /// <param name="cancellationToken">A cancellation token that can be used to cancel the request operation.</param>
  /// <returns>A task that represents the asynchronous operation of sending the form data. The task completes when the HTTP
  /// response is received.</returns>
  Task PostUrlEncodedFormAsync(
    string route,
    IEnumerable<KeyValuePair<string, string?>> elements,
    IEnumerable<RequestHeader>? additionalHeaders = default,
    CancellationToken cancellationToken = default);
}
