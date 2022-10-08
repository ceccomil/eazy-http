namespace EazyHttp.Contracts.Exceptions;

/// <summary>
/// Represents errors that occur when an http request returns a response code which is not a success.
/// </summary>
public class FailedRequestException : Exception
{
    /// <inheritdoc/>
    public HttpStatusCode StatusCode { get; }

    /// <inheritdoc/>
    public HttpMethod Method { get; }

    internal FailedRequestException(
        HttpStatusCode statusCode,
        HttpMethod method) : this(
            statusCode,
            method,
            null)
    { }

    internal FailedRequestException(
        HttpStatusCode statusCode,
        HttpMethod method,
        string? message) : this(
            statusCode,
            method,
            message,
            null)
    { }

    internal FailedRequestException(
        HttpStatusCode statusCode,
        HttpMethod method,
        string? message,
        Exception? innerException) : base(
            message,
            innerException)
    {
        StatusCode = statusCode;
        Method = method;
    }
}
