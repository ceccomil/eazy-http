namespace EazyHttp.Contracts.Exceptions;

/// <summary>
/// Represents errors that occur when a response content is expected to be an array of bytes
/// </summary>
public class ByteArrayExpectedException : Exception
{
    ///<inheritdoc/>
    public ByteArrayExpectedException() : base()
    { }

    ///<inheritdoc/>
    public ByteArrayExpectedException(
        string? message) : base(message)
    { }

    ///<inheritdoc/>
    public ByteArrayExpectedException(
        string? message,
        Exception? innerException) : base(
            message,
            innerException)
    { }
}

