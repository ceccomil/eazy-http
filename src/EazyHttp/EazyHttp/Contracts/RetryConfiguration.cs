namespace EazyHttp.Contracts;

/// <summary>
/// TODO documentation
/// </summary>
public class RetryConfiguration
{
    private int _maxAttempts = 1;

    /// <summary>
    /// The number of possible retries matching a valid failure condition
    /// </summary>
    public int MaxAttempts
    {
        get => _maxAttempts;
        set
        {
            if (value <= 0)
            {
                value = 1;
            }

            _maxAttempts = value;
        }
    }

    /// <summary>
    /// Optional seed value used when randomizing delays.
    /// </summary>
    public int? Seed { get; set; }

    /// <summary>
    /// TODO documentation
    /// </summary>
    public Func<HttpStatusCode, HttpMethod, bool> StatusCodeMatchingCondition { get; set; }

    /// <summary>
    /// Initializes a new instance of the <see cref="RetryConfiguration"/> class.
    /// </summary>
    public RetryConfiguration()
    {
        StatusCodeMatchingCondition = (status, _) =>
        {
            if (status is HttpStatusCode.ServiceUnavailable
                or HttpStatusCode.TooManyRequests)
            {
                return true;
            }

            return false;
        };
    }
}
