namespace EazyHttp.Runtime;

/// <summary>
/// Provides methods for executing HTTP requests with configurable retry logic and exponential backoff.
/// </summary>
/// <remarks>This class is intended for scenarios where transient HTTP failures may occur and automatic retries
/// are desirable. It supports retrying requests based on configurable conditions, such as specific HTTP status codes or
/// exceptions. All methods are thread-safe and can be used concurrently.</remarks>
public static class HttpRetryExecutor
{
  /// <summary>
  /// Sends an HTTP request asynchronously with optional retry logic based on the specified configuration.
  /// </summary>
  /// <remarks>If retry configuration is provided and the maximum number of attempts is greater than 1, the
  /// method retries the request on HTTP request exceptions or when the response status code matches the specified
  /// condition. Retries use exponential backoff with jitter. The request is not retried if the cancellation token is
  /// canceled.</remarks>
  /// <param name="client">The HTTP client used to send the request. Must not be null.</param>
  /// <param name="request">The HTTP request message to send. Must not be null.</param>
  /// <param name="config">The retry configuration that determines the number of attempts, backoff strategy, and status code matching logic.
  /// If null or if the maximum attempts is less than or equal to 1, the request is sent without retries.</param>
  /// <param name="cancellationToken">A cancellation token that can be used to cancel the operation.</param>
  /// <returns>A task that represents the asynchronous operation. The task result contains the HTTP response message returned by
  /// the server.</returns>
  public static Task<HttpResponseMessage> ExecuteAsync(
    HttpClient client,
    HttpRequestMessage request,
    RetryConfiguration? config,
    CancellationToken cancellationToken)
  {
    // No retry configured or effectively 1 attempt -> just send
    if (config is null || config.MaxAttempts <= 1)
    {
      return client.SendAsync(request, cancellationToken);
    }

    var retries = config.MaxAttempts - 1;
    
    if (retries <= 0)
    {
      return client.SendAsync(request, cancellationToken);
    }

    var random = config.Seed is int seed
      ? new Random(seed)
      : Random.Shared;

    var policy = Policy
      .Handle<HttpRequestException>()
      .OrResult<HttpResponseMessage>(x =>
      {
        var method = x.RequestMessage?.Method 
          ?? HttpMethod.Get;
        
        return config.StatusCodeMatchingCondition(
          x.StatusCode, 
          method);
      })
      .WaitAndRetryAsync(
        retries,
        attempt =>
        {
          // simple exponential backoff + jitter
          var baseDelay = TimeSpan.FromMilliseconds(200 * Math.Pow(2, attempt - 1));
          var jitter = TimeSpan.FromMilliseconds(random.Next(0, 200));
          return baseDelay + jitter;
        });

    return policy.ExecuteAsync((_, ct) => 
      client.SendAsync(request, ct),
      [],
      cancellationToken);
  }
}
