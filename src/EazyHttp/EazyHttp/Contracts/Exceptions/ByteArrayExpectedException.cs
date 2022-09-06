namespace EazyHttp.Contracts.Exceptions;

/// <summary>
/// TODO Documentation
/// </summary>
public class ByteArrayExpectedException : Exception
{
    /// <summary>
    /// TODO documentation
    /// </summary>
    public ByteArrayExpectedException() : base()
    { }

    /// <summary>
    /// TODO documentation
    /// </summary>
    public ByteArrayExpectedException(
        string? message) : base(message)
    { }

    /// <summary>
    /// TODO documentation
    /// </summary>
    public ByteArrayExpectedException(
        string? message,
        Exception? innerException) : base(
            message,
            innerException)
    { }
}

