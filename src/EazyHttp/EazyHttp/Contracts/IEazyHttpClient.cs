namespace EazyHttp.Contracts;

/// <summary>
/// Specifies the contract for an EazyHttpClient implementaion.
/// </summary>
public interface IEazyHttpClient
{
    /// <inheritdoc/>
    HttpClient HttpClient { get; }

    /// <summary>
    /// Latest response results.
    /// </summary>
    public ResponseResultsCollection ResponseResults { get; }

    /// <summary>
    /// Gets the status code of the last HTTP request 
    /// </summary>
    int ResponseCode { get; }

    /// <summary>
    /// Gets the status description of the last HTTP request 
    /// </summary>
    string ResponseStatus { get; }

    /// <summary>
    /// If available, gets the content type header of the last HTTP request
    /// </summary>
    MediaTypeHeaderValue? ResponseContentType { get; }

    /// <summary>
    /// Gets the content type header's string representation of the last HTTP request
    /// </summary>
    string ResponseContentTypeDescription { get; }

    /// <summary>
    /// If available, gets the content disposition header of the last HTTP request
    /// </summary>
    ContentDispositionHeaderValue? ResponseContentDisposition { get; }

    /// <summary>
    /// Send a GET request and returns a deserialized <typeparamref name="TResult"/>.
    /// </summary>
    /// <returns>A <typeparamref name="TResult"/>.</returns>
    /// <param name="route">Absolute or relative URI (relative URI depends on base address).</param>
    /// <param name="query">Optional query parameters.</param>
    /// <param name="authHeader">Optional authentication header.</param>
    /// <param name="additionalHeaders">Optional collection of headers added to the request.</param>
    /// <param name="cancellationToken">
    /// The cancellation token that can be used to cancel the operation.
    /// </param>
    /// <exception cref="FailedRequestException">
    /// </exception>
    /// <exception cref="AggregateException">
    /// </exception>
    /// <exception cref="ByteArrayExpectedException">
    /// </exception>
    Task<TResult?> GetAsync<TResult>(
        string route,
        HttpQuery? query = default,
        AuthenticationHeaderValue? authHeader = default,
        IEnumerable<RequestHeader>? additionalHeaders = default,
        CancellationToken cancellationToken = default) where TResult : class;

    /// <summary>
    /// Send a GET request and discard the returned content if any.
    /// </summary>
    /// <param name="route">Absolute or relative URI (relative URI depends on base address).</param>
    /// <param name="query">Optional query parameters.</param>
    /// <param name="authHeader">Optional authentication header.</param>
    /// <param name="additionalHeaders">Optional collection of headers added to the request.</param>
    /// <param name="cancellationToken">
    /// The cancellation token that can be used to cancel the operation.
    /// </param>
    /// <exception cref="FailedRequestException">
    /// </exception>
    /// <exception cref="AggregateException">
    /// </exception>
    /// <exception cref="ByteArrayExpectedException">
    /// </exception>
    Task GetAsync(
        string route,
        HttpQuery? query = default,
        AuthenticationHeaderValue? authHeader = default,
        IEnumerable<RequestHeader>? additionalHeaders = default,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Send a PUT request and returns a deserialized <typeparamref name="TResult"/>.
    /// </summary>
    /// <returns>A <typeparamref name="TResult"/>.</returns>
    /// <param name="route">Absolute or relative URI (relative URI depends on base address).</param>
    /// <param name="body">The body of the request.</param>
    /// <param name="authHeader">Optional authentication header.</param>
    /// <param name="additionalHeaders">Optional collection of headers added to the request.</param>
    /// <param name="cancellationToken">
    /// The cancellation token that can be used to cancel the operation.
    /// </param>
    /// <exception cref="FailedRequestException">
    /// </exception>
    /// <exception cref="AggregateException">
    /// </exception>
    /// <exception cref="ByteArrayExpectedException">
    /// </exception>
    Task<TResult?> PutAsync<TResult>(
        string route,
        object body,
        AuthenticationHeaderValue? authHeader = default,
        IEnumerable<RequestHeader>? additionalHeaders = default,
        CancellationToken cancellationToken = default) where TResult : class;

    /// <summary>
    /// Send a PUT request and discard the returned content if any.
    /// </summary>
    /// <param name="route">Absolute or relative URI (relative URI depends on base address).</param>
    /// <param name="body">The body of the request.</param>
    /// <param name="authHeader">Optional authentication header.</param>
    /// <param name="additionalHeaders">Optional collection of headers added to the request.</param>
    /// <param name="cancellationToken">
    /// The cancellation token that can be used to cancel the operation.
    /// </param>
    /// <exception cref="FailedRequestException">
    /// </exception>
    /// <exception cref="AggregateException">
    /// </exception>
    /// <exception cref="ByteArrayExpectedException">
    /// </exception>
    Task PutAsync(
        string route,
        object body,
        AuthenticationHeaderValue? authHeader = default,
        IEnumerable<RequestHeader>? additionalHeaders = default,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Send a POST request and returns a deserialized <typeparamref name="TResult"/>.
    /// </summary>
    /// <returns>A <typeparamref name="TResult"/>.</returns>
    /// <param name="route">Absolute or relative URI (relative URI depends on base address).</param>
    /// <param name="body">The body of the request.</param>
    /// <param name="authHeader">Optional authentication header.</param>
    /// <param name="additionalHeaders">Optional collection of headers added to the request.</param>
    /// <param name="cancellationToken">
    /// The cancellation token that can be used to cancel the operation.
    /// </param>
    /// <exception cref="FailedRequestException">
    /// </exception>
    /// <exception cref="AggregateException">
    /// </exception>
    /// <exception cref="ByteArrayExpectedException">
    /// </exception>
    Task<TResult?> PostAsync<TResult>(
        string route,
        object body,
        AuthenticationHeaderValue? authHeader = default,
        IEnumerable<RequestHeader>? additionalHeaders = default,
        CancellationToken cancellationToken = default) where TResult : class;

    /// <summary>
    /// Send a POST request and discard the returned content if any.
    /// </summary>
    /// <param name="route">Absolute or relative URI (relative URI depends on base address).</param>
    /// <param name="body">The body of the request.</param>
    /// <param name="authHeader">Optional authentication header.</param>
    /// <param name="additionalHeaders">Optional collection of headers added to the request.</param>
    /// <param name="cancellationToken">
    /// The cancellation token that can be used to cancel the operation.
    /// </param>
    /// <exception cref="FailedRequestException">
    /// </exception>
    /// <exception cref="AggregateException">
    /// </exception>
    /// <exception cref="ByteArrayExpectedException">
    /// </exception>
    Task PostAsync(
        string route,
        object body,
        AuthenticationHeaderValue? authHeader = default,
        IEnumerable<RequestHeader>? additionalHeaders = default,
        CancellationToken cancellationToken = default);


    /// <summary>
    /// Send a DELETE request and returns a deserialized <typeparamref name="TResult"/>.
    /// </summary>
    /// <returns>A <typeparamref name="TResult"/>.</returns>
    /// <param name="route">Absolute or relative URI (relative URI depends on base address).</param>
    /// <param name="query">Optional query parameters.</param>
    /// <param name="authHeader">Optional authentication header.</param>
    /// <param name="additionalHeaders">Optional collection of headers added to the request.</param>
    /// <param name="cancellationToken">
    /// The cancellation token that can be used to cancel the operation.
    /// </param>
    /// <exception cref="FailedRequestException">
    /// </exception>
    /// <exception cref="AggregateException">
    /// </exception>
    /// <exception cref="ByteArrayExpectedException">
    /// </exception>
    Task<TResult?> DeleteAsync<TResult>(
        string route,
        HttpQuery? query = default,
        AuthenticationHeaderValue? authHeader = default,
        IEnumerable<RequestHeader>? additionalHeaders = default,
        CancellationToken cancellationToken = default) where TResult : class;

    /// <summary>
    /// Send a DELETE request and discard the returned content if any.
    /// </summary>
    /// <param name="route">Absolute or relative URI (relative URI depends on base address).</param>
    /// <param name="query">Optional query parameters.</param>
    /// <param name="authHeader">Optional authentication header.</param>
    /// <param name="additionalHeaders">Optional collection of headers added to the request.</param>
    /// <param name="cancellationToken">
    /// The cancellation token that can be used to cancel the operation.
    /// </param>
    /// <exception cref="FailedRequestException">
    /// </exception>
    /// <exception cref="AggregateException">
    /// </exception>
    /// <exception cref="ByteArrayExpectedException">
    /// </exception>
    Task DeleteAsync(
        string route,
        HttpQuery? query = default,
        AuthenticationHeaderValue? authHeader = default,
        IEnumerable<RequestHeader>? additionalHeaders = default,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Send a PATCH request and returns a deserialized <typeparamref name="TResult"/>.
    /// </summary>
    /// <returns>A <typeparamref name="TResult"/>.</returns>
    /// <param name="route">Absolute or relative URI (relative URI depends on base address).</param>
    /// <param name="body">The body of the request.</param>
    /// <param name="authHeader">Optional authentication header.</param>
    /// <param name="additionalHeaders">Optional collection of headers added to the request.</param>
    /// <param name="cancellationToken">
    /// The cancellation token that can be used to cancel the operation.
    /// </param>
    /// <exception cref="FailedRequestException">
    /// </exception>
    /// <exception cref="AggregateException">
    /// </exception>
    /// <exception cref="ByteArrayExpectedException">
    /// </exception>
    Task<TResult?> PatchAsync<TResult>(
        string route,
        object body,
        AuthenticationHeaderValue? authHeader = default,
        IEnumerable<RequestHeader>? additionalHeaders = default,
        CancellationToken cancellationToken = default) where TResult : class;

    /// <summary>
    /// Send a PATCH request and discard the returned content if any.
    /// </summary>
    /// <param name="route">Absolute or relative URI (relative URI depends on base address).</param>
    /// <param name="body">The body of the request.</param>
    /// <param name="authHeader">Optional authentication header.</param>
    /// <param name="additionalHeaders">Optional collection of headers added to the request.</param>
    /// <param name="cancellationToken">
    /// The cancellation token that can be used to cancel the operation.
    /// </param>
    /// <exception cref="FailedRequestException">
    /// </exception>
    /// <exception cref="AggregateException">
    /// </exception>
    /// <exception cref="ByteArrayExpectedException">
    /// </exception>
    Task PatchAsync(
        string route,
        object body,
        AuthenticationHeaderValue? authHeader = default,
        IEnumerable<RequestHeader>? additionalHeaders = default,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Send a POST (multipart/form-data) request and returns a deserialized <typeparamref name="TResult"/>.
    /// </summary>
    /// <returns>A <typeparamref name="TResult"/>.</returns>
    /// <param name="route">Absolute or relative URI (relative URI depends on base address).</param>
    /// <param name="elements">The body of the request.</param>
    /// <param name="authHeader">Optional authentication header.</param>
    /// <param name="additionalHeaders">Optional collection of headers added to the request.</param>
    /// <param name="cancellationToken">
    /// The cancellation token that can be used to cancel the operation.
    /// </param>
    /// <exception cref="FailedRequestException">
    /// </exception>
    /// <exception cref="AggregateException">
    /// </exception>
    /// <exception cref="ByteArrayExpectedException">
    /// </exception>
    Task<TResult?> PostFormAsync<TResult>(
        string route,
        IEnumerable<FormElement> elements,
        AuthenticationHeaderValue? authHeader = default,
        IEnumerable<RequestHeader>? additionalHeaders = default,
        CancellationToken cancellationToken = default) where TResult : class;

    /// <summary>
    /// Send a POST (multipart/form-data) request and discard the returned content if any.
    /// </summary>
    /// <param name="route">Absolute or relative URI (relative URI depends on base address).</param>
    /// <param name="elements">The body of the request.</param>
    /// <param name="authHeader">Optional authentication header.</param>
    /// <param name="additionalHeaders">Optional collection of headers added to the request.</param>
    /// <param name="cancellationToken">
    /// The cancellation token that can be used to cancel the operation.
    /// </param>
    /// <exception cref="FailedRequestException">
    /// </exception>
    /// <exception cref="AggregateException">
    /// </exception>
    /// <exception cref="ByteArrayExpectedException">
    /// </exception>
    Task PostFormAsync(
        string route,
        IEnumerable<FormElement> elements,
        AuthenticationHeaderValue? authHeader = default,
        IEnumerable<RequestHeader>? additionalHeaders = default,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Send a POST (application/x-www-form-urlencoded) request and returns a deserialized <typeparamref name="TResult"/>.
    /// </summary>
    /// <returns>A <typeparamref name="TResult"/>.</returns>
    /// <param name="route">Absolute or relative URI (relative URI depends on base address).</param>
    /// <param name="elements">The body of the request.</param>
    /// <param name="authHeader">Optional authentication header.</param>
    /// <param name="additionalHeaders">Optional collection of headers added to the request.</param>
    /// <param name="cancellationToken">
    /// The cancellation token that can be used to cancel the operation.
    /// </param>
    /// <exception cref="FailedRequestException">
    /// </exception>
    /// <exception cref="AggregateException">
    /// </exception>
    /// <exception cref="ByteArrayExpectedException">
    /// </exception>
    Task<TResult?> PostUrlEncodedFormAsync<TResult>(
        string route,
        IEnumerable<KeyValuePair<string, string?>> elements,
        AuthenticationHeaderValue? authHeader = default,
        IEnumerable<RequestHeader>? additionalHeaders = default,
        CancellationToken cancellationToken = default) where TResult : class;

    /// <summary>
    /// Send a POST (application/x-www-form-urlencoded) request and discard the returned content if any.
    /// </summary>
    /// <param name="route">Absolute or relative URI (relative URI depends on base address).</param>
    /// <param name="elements">The body of the request.</param>
    /// <param name="authHeader">Optional authentication header.</param>
    /// <param name="additionalHeaders">Optional collection of headers added to the request.</param>
    /// <param name="cancellationToken">
    /// The cancellation token that can be used to cancel the operation.
    /// </param>
    /// <exception cref="FailedRequestException">
    /// </exception>
    /// <exception cref="AggregateException">
    /// </exception>
    /// <exception cref="ByteArrayExpectedException">
    /// </exception>
    Task PostUrlEncodedFormAsync(
        string route,
        IEnumerable<KeyValuePair<string, string?>> elements,
        AuthenticationHeaderValue? authHeader = default,
        IEnumerable<RequestHeader>? additionalHeaders = default,
        CancellationToken cancellationToken = default);
}
