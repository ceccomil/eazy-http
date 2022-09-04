namespace EazyHttp.Contracts.Exceptions;

/// <summary>
/// TODO Documentation
/// </summary>
public class FailedRequestException : Exception
{
    /// <summary>
    /// TODO documentation
    /// </summary>
    public FailedRequestException() : base()
    { }

    /// <summary>
    /// TODO documentation
    /// </summary>
    public FailedRequestException(
        string? message) : base(message)
    { }

    /// <summary>
    /// TODO documentation
    /// </summary>
    public FailedRequestException(
        string? message,
        Exception? innerException) : base(
            message,
            innerException)
    { }
}
