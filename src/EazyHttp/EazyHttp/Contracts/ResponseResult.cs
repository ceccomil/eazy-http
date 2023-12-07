namespace EazyHttp.Contracts;

/// <summary>
/// Represents the result of a request.
/// </summary>
public sealed class ResponseResult
{
    /// <summary>
    /// 
    /// </summary>
    public DateTimeOffset ResponseTime { get; } = DateTimeOffset
        .UtcNow;

    /// <summary>
    /// 
    /// </summary>
    public int ResponseCode { get; internal set; }

    /// <summary>
    /// 
    /// </summary>
    public string ResponseStatus { get; internal set; } = "Unknown";

    /// <summary>
    /// 
    /// </summary>
    public MediaTypeHeaderValue? ResponseContentType { get; internal set; }

    /// <summary>
    /// 
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
    /// 
    /// </summary>
    public ContentDispositionHeaderValue? ResponseContentDisposition { get; internal set; }
}
