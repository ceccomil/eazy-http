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
  /// <summary>
  /// Gets the underlying <see cref="HttpClient"/> used to send requests.
  /// </summary>
  HttpClient HttpClient { get; }

  // -------- GET --------

  /// <summary>
  /// Sends an asynchronous HTTP GET request to the specified route and returns the response deserialized to the
  /// specified type.
  /// </summary>
  /// <remarks>
  /// The method will throw an exception if the request fails due to network errors or if deserialization fails.
  /// Callers are responsible for handling cancellation and exceptions as appropriate.
  /// </remarks>
  /// <typeparam name="TResult">
  /// The type to which the HTTP response content will be deserialized. Must be compatible with the response format.
  /// </typeparam>
  /// <param name="route">
  /// The relative or absolute URI route to which the GET request is sent. Cannot be <see langword="null"/> or empty.
  /// </param>
  /// <param name="query">
  /// An optional query object used to append query parameters to the request URI. If <see langword="null"/>,
  /// no query parameters are added.
  /// </param>
  /// <param name="cancellationToken">
  /// A token that can be used to cancel the asynchronous operation.
  /// </param>
  /// <returns>
  /// A task that represents the asynchronous operation. The task result contains the deserialized response of type
  /// <typeparamref name="TResult"/> if the request is successful; otherwise, <see langword="null"/>.
  /// </returns>
  Task<TResult?> GetAsync<TResult>(
    string route,
    HttpQuery? query = default,
    CancellationToken cancellationToken = default);

  /// <summary>
  /// Sends an asynchronous HTTP GET request to the specified route and returns a response envelope containing the
  /// deserialized result and HTTP metadata.
  /// </summary>
  /// <remarks>
  /// This method does not throw for non-success HTTP status codes. Instead, the status code and any error information
  /// are included in the returned <see cref="ResponseEnvelope{TResult}"/>.
  /// </remarks>
  /// <typeparam name="TResult">
  /// The type to which the response content will be deserialized. Must be compatible with the response payload.
  /// </typeparam>
  /// <param name="route">
  /// The relative route or endpoint to which the GET request will be sent. Cannot be <see langword="null"/> or empty.
  /// </param>
  /// <param name="query">
  /// An optional query object representing URL parameters to include in the request. If <see langword="null"/>,
  /// no query parameters are added.
  /// </param>
  /// <param name="cancellationToken">
  /// A cancellation token that can be used to cancel the request.
  /// </param>
  /// <returns>
  /// A task that represents the asynchronous operation. The task result contains a response envelope with the
  /// deserialized result of type <typeparamref name="TResult"/>. The result may be <see langword="null"/> if the
  /// response contains no content.
  /// </returns>
  Task<ResponseEnvelope<TResult?>> GetWithResponseAsync<TResult>(
    string route,
    HttpQuery? query = default,
    CancellationToken cancellationToken = default);

  /// <summary>
  /// Sends an asynchronous HTTP GET request to the specified route with optional query parameters and no response body.
  /// </summary>
  /// <param name="route">
  /// The relative or absolute route to which the GET request will be sent. Cannot be <see langword="null"/> or empty.
  /// </param>
  /// <param name="query">
  /// An optional set of query parameters to include in the request. If <see langword="null"/>,
  /// no query parameters are added.
  /// </param>
  /// <param name="cancellationToken">
  /// A token that can be used to cancel the asynchronous operation.
  /// </param>
  /// <returns>A task that represents the asynchronous GET operation.</returns>
  Task GetAsync(
    string route,
    HttpQuery? query = default,
    CancellationToken cancellationToken = default);

  // -------- PUT --------

  /// <summary>
  /// Sends an asynchronous HTTP PUT request to the specified route with the provided request body
  /// and returns the deserialized response.
  /// </summary>
  /// <remarks>
  /// The request body is serialized according to the configured content type. If the server response
  /// cannot be deserialized to <typeparamref name="TResult"/>, the result will be <see langword="null"/>.
  /// </remarks>
  /// <typeparam name="TResult">
  /// The type to which the response content will be deserialized. Must be compatible with the response format.
  /// </typeparam>
  /// <param name="route">
  /// The relative or absolute URI of the endpoint to which the PUT request is sent. Cannot be <see langword="null"/> or empty.
  /// </param>
  /// <param name="body">
  /// The object representing the request payload to be serialized and sent in the PUT request body. Cannot be <see langword="null"/>.
  /// </param>
  /// <param name="cancellationToken">
  /// A token to monitor for cancellation requests. The operation is canceled if the token is triggered.
  /// </param>
  /// <returns>
  /// A task that represents the asynchronous operation. The task result contains the deserialized response of type
  /// <typeparamref name="TResult"/> if the request is successful; otherwise, <see langword="null"/>.
  /// </returns>
  Task<TResult?> PutAsync<TResult>(
    string route,
    object body,
    CancellationToken cancellationToken = default);

  /// <summary>
  /// Sends an HTTP PUT request to the specified route with the provided request body and returns a response envelope
  /// containing the deserialized result and HTTP metadata.
  /// </summary>
  /// <typeparam name="TResult">The type to which the response content will be deserialized.</typeparam>
  /// <param name="route">
  /// The relative URI route to which the PUT request is sent. Cannot be <see langword="null"/> or empty.
  /// </param>
  /// <param name="body">
  /// The object representing the request payload to be serialized and sent in the PUT request. Cannot be <see langword="null"/>.
  /// </param>
  /// <param name="cancellationToken">
  /// A cancellation token that can be used to cancel the request.
  /// </param>
  /// <returns>
  /// A task that represents the asynchronous operation. The task result contains a response envelope with the
  /// deserialized result of type <typeparamref name="TResult"/>. The result may be <see langword="null"/> if the
  /// response contains no content.
  /// </returns>
  Task<ResponseEnvelope<TResult?>> PutWithResponseAsync<TResult>(
    string route,
    object body,
    CancellationToken cancellationToken = default);

  /// <summary>
  /// Sends an asynchronous HTTP PUT request to the specified route with the provided request body.
  /// </summary>
  /// <remarks>
  /// The request body is serialized according to the implementation's configuration. The method does
  /// not return a value; it completes when the response has been received.
  /// </remarks>
  /// <param name="route">
  /// The relative or absolute URI of the endpoint to which the PUT request is sent. Cannot be <see langword="null"/> or empty.
  /// </param>
  /// <param name="body">
  /// The object to serialize and include as the request body. Cannot be <see langword="null"/>.
  /// </param>
  /// <param name="cancellationToken">
  /// A token that can be used to cancel the operation.
  /// </param>
  /// <returns>A task that represents the asynchronous operation of sending the PUT request.</returns>
  Task PutAsync(
    string route,
    object body,
    CancellationToken cancellationToken = default);

  // -------- POST --------

  /// <summary>
  /// Sends an HTTP POST request to the specified route with the provided request body and
  /// asynchronously returns the deserialized response.
  /// </summary>
  /// <remarks>
  /// The request body is serialized according to the configured content type (e.g., JSON).
  /// If the response cannot be deserialized to <typeparamref name="TResult"/>, the result will be <see langword="null"/>.
  /// </remarks>
  /// <typeparam name="TResult">
  /// The type to which the response content will be deserialized. Must be compatible with the response format.
  /// </typeparam>
  /// <param name="route">
  /// The relative or absolute URI of the endpoint to which the POST request is sent. Cannot be <see langword="null"/> or empty.
  /// </param>
  /// <param name="body">
  /// The object representing the request payload to be serialized and sent in the POST request body. Cannot be <see langword="null"/>.
  /// </param>
  /// <param name="cancellationToken">
  /// A token to monitor for cancellation requests. The operation is canceled if the token is triggered.
  /// </param>
  /// <returns>
  /// A task that represents the asynchronous operation. The task result contains the deserialized response of type
  /// <typeparamref name="TResult"/> if the request is successful; otherwise, <see langword="null"/>.
  /// </returns>
  Task<TResult?> PostAsync<TResult>(
    string route,
    object body,
    CancellationToken cancellationToken = default);

  /// <summary>
  /// Sends an HTTP POST request to the specified route with the provided request body and returns the deserialized
  /// response envelope containing the result and HTTP metadata.
  /// </summary>
  /// <typeparam name="TResult">The type to which the response content will be deserialized.</typeparam>
  /// <param name="route">
  /// The relative route or endpoint to which the POST request is sent. Cannot be <see langword="null"/> or empty.
  /// </param>
  /// <param name="body">
  /// The object representing the request payload to be serialized and sent in the POST request body. Cannot be <see langword="null"/>.
  /// </param>
  /// <param name="cancellationToken">
  /// A cancellation token that can be used to cancel the request.
  /// </param>
  /// <returns>
  /// A task that represents the asynchronous operation. The task result contains a <see cref="ResponseEnvelope{TResult}"/>
  /// holding the deserialized result of type <typeparamref name="TResult"/>, or <see langword="null"/> if the response has
  /// no content.
  /// </returns>
  Task<ResponseEnvelope<TResult?>> PostWithResponseAsync<TResult>(
    string route,
    object body,
    CancellationToken cancellationToken = default);

  /// <summary>
  /// Sends an asynchronous HTTP POST request to the specified route with the provided request body.
  /// </summary>
  /// <param name="route">
  /// The relative route or endpoint to which the POST request is sent. Cannot be <see langword="null"/> or empty.
  /// </param>
  /// <param name="body">
  /// The object representing the request payload to be serialized and sent in the body of the POST request.
  /// Cannot be <see langword="null"/>.
  /// </param>
  /// <param name="cancellationToken">
  /// A token that can be used to cancel the operation.
  /// </param>
  /// <returns>A task that represents the asynchronous operation of sending the POST request.</returns>
  Task PostAsync(
    string route,
    object body,
    CancellationToken cancellationToken = default);

  // -------- DELETE --------

  /// <summary>
  /// Sends an asynchronous HTTP DELETE request to the specified route and returns the response deserialized to the
  /// specified type.
  /// </summary>
  /// <typeparam name="TResult">
  /// The type to which the response content will be deserialized. Must be compatible with the response format.
  /// </typeparam>
  /// <param name="route">
  /// The relative or absolute URI route to which the DELETE request is sent. Cannot be <see langword="null"/> or empty.
  /// </param>
  /// <param name="query">
  /// An optional query object used to append query parameters to the request URI. If <see langword="null"/>,
  /// no query parameters are added.
  /// </param>
  /// <param name="cancellationToken">
  /// A cancellation token that can be used to cancel the request.
  /// </param>
  /// <returns>
  /// A task that represents the asynchronous operation. The task result contains the deserialized response of type
  /// <typeparamref name="TResult"/> if the request is successful; otherwise, <see langword="null"/>.
  /// </returns>
  Task<TResult?> DeleteAsync<TResult>(
    string route,
    HttpQuery? query = default,
    CancellationToken cancellationToken = default);

  /// <summary>
  /// Sends an HTTP DELETE request to the specified route and returns the server's response, including any deserialized
  /// result data and HTTP metadata.
  /// </summary>
  /// <typeparam name="TResult">
  /// The type of the result expected from the server's response. Must be compatible with the response payload, or
  /// nullable if no content is expected.
  /// </typeparam>
  /// <param name="route">
  /// The relative route or endpoint to which the DELETE request is sent. Cannot be <see langword="null"/> or empty.
  /// </param>
  /// <param name="query">
  /// An optional set of query parameters to include in the request URL. If <see langword="null"/>,
  /// no query parameters are added.
  /// </param>
  /// <param name="cancellationToken">
  /// A cancellation token that can be used to cancel the request operation.
  /// </param>
  /// <returns>
  /// A task that represents the asynchronous operation. The result contains a response envelope with the server's
  /// response and the deserialized result of type <typeparamref name="TResult"/> if available; otherwise, <see langword="null"/>.
  /// </returns>
  Task<ResponseEnvelope<TResult?>> DeleteWithResponseAsync<TResult>(
    string route,
    HttpQuery? query = default,
    CancellationToken cancellationToken = default);

  /// <summary>
  /// Sends an asynchronous HTTP DELETE request to the specified route.
  /// </summary>
  /// <param name="route">
  /// The relative URI route to which the DELETE request will be sent. Cannot be <see langword="null"/> or empty.
  /// </param>
  /// <param name="query">
  /// An optional query object used to append query parameters to the route. If <see langword="null"/>,
  /// no query parameters are included.
  /// </param>
  /// <param name="cancellationToken">
  /// A cancellation token that can be used to cancel the operation.
  /// </param>
  /// <returns>A task that represents the asynchronous delete operation.</returns>
  Task DeleteAsync(
    string route,
    HttpQuery? query = default,
    CancellationToken cancellationToken = default);

  // -------- PATCH --------

  /// <summary>
  /// Sends an asynchronous HTTP PATCH request to the specified route with the provided request body and returns the
  /// deserialized response.
  /// </summary>
  /// <typeparam name="TResult">
  /// The type to which the response content will be deserialized. Must be compatible with the response format.
  /// </typeparam>
  /// <param name="route">
  /// The relative route or endpoint to which the PATCH request is sent. Cannot be <see langword="null"/> or empty.
  /// </param>
  /// <param name="body">
  /// The object representing the request payload to be serialized and sent in the PATCH request body.
  /// Cannot be <see langword="null"/>.
  /// </param>
  /// <param name="cancellationToken">
  /// A cancellation token that can be used to cancel the request.
  /// </param>
  /// <returns>
  /// A task that represents the asynchronous operation. The task result contains the deserialized response of type
  /// <typeparamref name="TResult"/> if the request is successful; otherwise, <see langword="null"/>.
  /// </returns>
  Task<TResult?> PatchAsync<TResult>(
    string route,
    object body,
    CancellationToken cancellationToken = default);

  /// <summary>
  /// Sends an HTTP PATCH request to the specified route with the provided request body and returns the deserialized
  /// response envelope asynchronously.
  /// </summary>
  /// <typeparam name="TResult">
  /// The type to which the response content will be deserialized. Must be compatible with the response payload.
  /// </typeparam>
  /// <param name="route">
  /// The relative URI route of the endpoint to which the PATCH request is sent. Cannot be <see langword="null"/> or empty.
  /// </param>
  /// <param name="body">
  /// The object representing the request payload to be serialized and sent in the PATCH request body.
  /// Cannot be <see langword="null"/>.
  /// </param>
  /// <param name="cancellationToken">
  /// A cancellation token that can be used to cancel the request operation.
  /// </param>
  /// <returns>
  /// A task that represents the asynchronous operation. The task result contains a response envelope with the
  /// deserialized response of type <typeparamref name="TResult"/>. The result may be <see langword="null"/> if the
  /// response contains no content.
  /// </returns>
  Task<ResponseEnvelope<TResult?>> PatchWithResponseAsync<TResult>(
    string route,
    object body,
    CancellationToken cancellationToken = default);

  /// <summary>
  /// Sends an asynchronous HTTP PATCH request to the specified route with the provided request body.
  /// </summary>
  /// <param name="route">
  /// The relative or absolute URI of the endpoint to which the PATCH request is sent. Cannot be <see langword="null"/> or empty.
  /// </param>
  /// <param name="body">
  /// The object representing the content to be serialized and included in the PATCH request body.
  /// Cannot be <see langword="null"/>.
  /// </param>
  /// <param name="cancellationToken">
  /// A token that can be used to cancel the PATCH request operation.
  /// </param>
  /// <returns>A task that represents the asynchronous PATCH operation.</returns>
  Task PatchAsync(
    string route,
    object body,
    CancellationToken cancellationToken = default);

  // -------- multipart/form-data --------

  /// <summary>
  /// Sends an HTTP POST request with multipart/form-data content to the specified route and returns the deserialized
  /// response.
  /// </summary>
  /// <remarks>
  /// The method serializes the provided form elements as multipart/form-data and deserializes the response
  /// content to the specified type. If the response cannot be deserialized, the result will be <see langword="null"/>.
  /// </remarks>
  /// <typeparam name="TResult">The type to which the response content will be deserialized.</typeparam>
  /// <param name="route">
  /// The relative or absolute URI of the endpoint to which the form data will be posted. Cannot be <see langword="null"/> or empty.
  /// </param>
  /// <param name="elements">
  /// A collection of form elements to include in the multipart/form-data request body. Each element represents a field
  /// or file to be sent.
  /// </param>
  /// <param name="cancellationToken">
  /// A token that can be used to cancel the request operation.
  /// </param>
  /// <returns>
  /// A task that represents the asynchronous operation. The task result contains the deserialized response of type
  /// <typeparamref name="TResult"/> if the request is successful; otherwise, <see langword="null"/>.
  /// </returns>
  Task<TResult?> PostFormAsync<TResult>(
    string route,
    IEnumerable<FormElement> elements,
    CancellationToken cancellationToken = default);

  /// <summary>
  /// Sends a POST request with multipart/form-data content to the specified route and returns the server's response
  /// wrapped in a response envelope.
  /// </summary>
  /// <typeparam name="TResult">
  /// The type of the expected result contained in the response envelope.
  /// </typeparam>
  /// <param name="route">
  /// The relative route to which the form data will be posted. Cannot be <see langword="null"/> or empty.
  /// </param>
  /// <param name="elements">
  /// A collection of form elements representing the data to include in the POST request. Cannot be <see langword="null"/>.
  /// </param>
  /// <param name="cancellationToken">
  /// A cancellation token that can be used to cancel the request.
  /// </param>
  /// <returns>
  /// A task that represents the asynchronous operation. The task result contains a response envelope with the
  /// deserialized result of type <typeparamref name="TResult"/> if the request is successful; otherwise, the envelope
  /// may contain error information.
  /// </returns>
  Task<ResponseEnvelope<TResult?>> PostFormWithResponseAsync<TResult>(
    string route,
    IEnumerable<FormElement> elements,
    CancellationToken cancellationToken = default);

  /// <summary>
  /// Sends an HTTP POST request to the specified route with multipart/form-data content.
  /// </summary>
  /// <param name="route">
  /// The relative or absolute URI route to which the form data will be posted. Cannot be <see langword="null"/> or empty.
  /// </param>
  /// <param name="elements">
  /// A collection of form elements to include in the POST request body. Cannot be <see langword="null"/>;
  /// should contain at least one element.
  /// </param>
  /// <param name="cancellationToken">
  /// A cancellation token that can be used to cancel the operation.
  /// </param>
  /// <returns>
  /// A task that represents the asynchronous operation of posting the form data.
  /// The task completes when the request has been sent and the response is received.
  /// </returns>
  Task PostFormAsync(
    string route,
    IEnumerable<FormElement> elements,
    CancellationToken cancellationToken = default);

  // -------- application/x-www-form-urlencoded --------

  /// <summary>
  /// Sends an HTTP POST request with application/x-www-form-urlencoded data to the specified route and returns the
  /// deserialized response.
  /// </summary>
  /// <remarks>
  /// The request is sent with the Content-Type header set to application/x-www-form-urlencoded.
  /// The method will deserialize the response content to the specified type using the configured deserializer.
  /// </remarks>
  /// <typeparam name="TResult">The type to which the response content will be deserialized.</typeparam>
  /// <param name="route">
  /// The relative or absolute URI of the endpoint to which the form data will be posted. Cannot be <see langword="null"/> or empty.
  /// </param>
  /// <param name="elements">
  /// A collection of key-value pairs representing the form fields and their values to include in the request body.
  /// Keys cannot be <see langword="null"/>; values may be <see langword="null"/> to indicate an empty field.
  /// </param>
  /// <param name="cancellationToken">
  /// A cancellation token that can be used to cancel the request.
  /// </param>
  /// <returns>
  /// A task that represents the asynchronous operation. The task result contains the deserialized response of type
  /// <typeparamref name="TResult"/>, or <see langword="null"/> if the response content is empty.
  /// </returns>
  Task<TResult?> PostUrlEncodedFormAsync<TResult>(
    string route,
    IEnumerable<KeyValuePair<string, string?>> elements,
    CancellationToken cancellationToken = default);

  /// <summary>
  /// Sends an HTTP POST request with URL-encoded form data to the specified route and asynchronously returns the
  /// deserialized response envelope.
  /// </summary>
  /// <typeparam name="TResult">The type to which the response content will be deserialized.</typeparam>
  /// <param name="route">
  /// The relative route or endpoint to which the POST request is sent. Cannot be <see langword="null"/> or empty.
  /// </param>
  /// <param name="elements">
  /// A collection of key-value pairs representing the form data to be URL-encoded and included in the request body.
  /// Keys cannot be <see langword="null"/>.
  /// </param>
  /// <param name="cancellationToken">
  /// A cancellation token that can be used to cancel the request operation.
  /// </param>
  /// <returns>
  /// A task that represents the asynchronous operation. The task result contains a
  /// <see cref="ResponseEnvelope{TResult}"/> with the deserialized response data.
  /// The <c>TResult</c> value may be <see langword="null"/> if the response is empty or cannot be deserialized.
  /// </returns>
  Task<ResponseEnvelope<TResult?>> PostUrlEncodedFormWithResponseAsync<TResult>(
    string route,
    IEnumerable<KeyValuePair<string, string?>> elements,
    CancellationToken cancellationToken = default);

  /// <summary>
  /// Sends an HTTP POST request with form data encoded as application/x-www-form-urlencoded to the specified route
  /// asynchronously.
  /// </summary>
  /// <param name="route">
  /// The relative or absolute URI of the endpoint to which the form data will be posted. Cannot be <see langword="null"/> or empty.
  /// </param>
  /// <param name="elements">
  /// A collection of key-value pairs representing the form fields and their values to include in the request body.
  /// Keys cannot be <see langword="null"/>; values may be <see langword="null"/> to indicate an empty field.
  /// </param>
  /// <param name="cancellationToken">
  /// A cancellation token that can be used to cancel the request operation.
  /// </param>
  /// <returns>
  /// A task that represents the asynchronous operation of sending the form data.
  /// The task completes when the HTTP response is received.
  /// </returns>
  Task PostUrlEncodedFormAsync(
    string route,
    IEnumerable<KeyValuePair<string, string?>> elements,
    CancellationToken cancellationToken = default);
}
