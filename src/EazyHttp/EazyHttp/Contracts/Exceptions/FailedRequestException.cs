namespace EazyHttp.Contracts.Exceptions;

/// <summary>
/// TODO Documentation
/// </summary>
public class FailedRequestException : Exception
{
    /// <inheritdoc/>
    public HttpStatusCode StatusCode { get; }

    /// <inheritdoc/>
    public HttpMethod Method { get; }

    /// <summary>
    /// TODO documentation
    /// </summary>
    public FailedRequestException(
        HttpStatusCode statusCode,
        HttpMethod method) : this(
            statusCode,
            method,
            null)
    { }

    /// <summary>
    /// TODO documentation
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
    /// TODO documentation
    /// </summary>
    public FailedRequestException(
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
