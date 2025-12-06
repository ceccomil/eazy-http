namespace EazyHttp.Contracts;

/// <summary>
/// Represents the configuration settings used to control retry behavior for operations, including the maximum number of
/// attempts and conditions for retrying based on HTTP status codes.
/// </summary>
/// <remarks>Use this class to specify how and when retry logic should be applied to operations that may fail due
/// to transient errors, such as network issues or service unavailability. The configuration allows customization of the
/// maximum number of retry attempts, an optional randomization seed, and a predicate to determine which HTTP status
/// codes and methods should trigger a retry. This type is immutable and thread-safe when used as a record.</remarks>
public sealed record RetryConfiguration
{
  private int _maxAttempts = 1;

  /// <summary>
  /// Gets or sets the maximum number of attempts allowed for an operation.
  /// </summary>
  /// <remarks>If a value less than or equal to 0 is assigned, the property is set to 1.</remarks>
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
  /// Gets or sets the function used to determine whether a given HTTP status code and method satisfy a matching
  /// condition.
  /// </summary>
  /// <remarks>The function should return <see langword="true"/> if the specified <see cref="HttpStatusCode"/>
  /// and <see cref="HttpMethod"/> meet the desired criteria; otherwise, <see langword="false"/>. This property allows
  /// customization of status code and method matching logic for HTTP operations.</remarks>
  public Func<HttpStatusCode, HttpMethod, bool> StatusCodeMatchingCondition { get; set; }

  /// <summary>
  /// Initializes a new instance of the RetryConfiguration class with default retry conditions for HTTP status codes.
  /// </summary>
  /// <remarks>By default, the retry condition matches HTTP 503 (Service Unavailable) and 429 (Too Many
  /// Requests) status codes. You can modify the StatusCodeMatchingCondition property after construction to customize
  /// which status codes should trigger a retry.</remarks>
  public RetryConfiguration()
  {
    StatusCodeMatchingCondition = static (status, _) =>
    {
      var numericalStatus = (int)status;

      if (numericalStatus == 503 ||
        numericalStatus == 429)
      {
        return true;
      }

      return false;
    };
  }
}
