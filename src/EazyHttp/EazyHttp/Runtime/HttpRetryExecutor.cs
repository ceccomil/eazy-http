namespace EazyHttp.Runtime;

/// <summary>
/// Provides factory methods for creating HTTP retry policies using Polly for resilient HTTP request execution.
/// </summary>
/// <remarks>This class is intended for use in scenarios where HTTP requests may fail transiently and should be
/// retried according to a configurable policy. The generated policies can be used with HTTP clients to automatically
/// handle retries for failed requests based on exceptions or HTTP response status codes. All members are static and
/// thread-safe.</remarks>
public static class HttpRetryExecutor
{
  /// <summary>
  /// Creates an asynchronous retry policy for HTTP requests based on the specified retry configuration.Hi
  /// </summary>
  /// <remarks>The returned policy retries on HTTP request exceptions and on HTTP responses that match the
  /// status code condition defined in the configuration. The delay between retries increases exponentially with each
  /// attempt and includes a random jitter to reduce contention. If the configuration's MaxAttempts is less than or
  /// equal to 1, no retries are performed.</remarks>
  /// <param name="config">The retry configuration that defines the maximum number of attempts, status code matching logic, and optional
  /// randomization seed. Cannot be null.</param>
  /// <returns>An asynchronous policy that retries failed HTTP requests according to the provided configuration. Returns a no-op
  /// policy if the configuration is null or specifies one or fewer attempts.</returns>
  public static IAsyncPolicy<HttpResponseMessage> CreatePolicy(
    RetryConfiguration? config)
  {
    if (config is null || config.MaxAttempts <= 1)
    {
      return Policy.NoOpAsync<HttpResponseMessage>();
    }

    var retries = config.MaxAttempts - 1;
    if (retries <= 0)
    {
      return Policy.NoOpAsync<HttpResponseMessage>();
    }

    return Policy
      .Handle<HttpRequestException>()
      .OrResult<HttpResponseMessage>(response =>
      {
        var method = response.RequestMessage?.Method ?? HttpMethod.Get;
        return config.StatusCodeMatchingCondition(response.StatusCode, method);
      })
      .WaitAndRetryAsync(
        retries,
        attempt =>
        {
          var random = config.Seed is int seed
            ? new Random(seed + attempt)
            : Random.Shared;

          var baseDelay = TimeSpan.FromMilliseconds(
            200 * Math.Pow(2, attempt - 1));

          var jitter = TimeSpan.FromMilliseconds(
            random.Next(0, 200));

          return baseDelay + jitter;
        });
  }
}