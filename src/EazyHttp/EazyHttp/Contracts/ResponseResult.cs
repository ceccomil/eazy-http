namespace EazyHttp.Contracts;

/// <summary>
/// Represents the result of a request.
/// </summary>
public sealed class ResponseResult
{
    /// <summary>
    /// The request identifier.
    /// </summary>
    public Guid Id { get; }

    /// <summary>
    /// The response timestamp.
    /// </summary>
    public DateTimeOffset ResponseTime { get; } = DateTimeOffset
        .UtcNow;

    /// <summary>
    /// The response code.
    /// </summary>
    public int ResponseCode { get; internal set; }

    /// <summary>
    /// The response status.
    /// </summary>
    public string ResponseStatus { get; internal set; } = "Unknown";

    /// <summary>
    /// The response content type.
    /// </summary>
    public MediaTypeHeaderValue? ResponseContentType { get; internal set; }

    /// <summary>
    /// The response content type description.
    /// </summary>
    public string ResponseContentTypeDescription
    {
        get
        {
            if (ResponseContentType is null)
            {
                return "Unknown";
            }

            return ResponseContentType
                .ToString();
        }
    }

    /// <summary>
    /// The response content disposition.
    /// </summary>
    public ContentDispositionHeaderValue? ResponseContentDisposition { get; internal set; }

    internal ResponseResult(Guid? id)
    {
        id ??= Guid
            .NewGuid();

        Id = id.Value;
    }
}
