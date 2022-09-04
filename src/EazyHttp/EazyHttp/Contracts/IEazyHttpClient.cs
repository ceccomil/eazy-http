namespace EazyHttp.Contracts;

/// <summary>
/// TODO documentation
/// </summary>
public interface IEazyHttpClient
{
    /// <summary>
    /// Gets the status code of the last HTTP request 
    /// </summary>
    int ResponseCode { get; }

    /// <summary>
    /// Gets the status description of the last HTTP request 
    /// </summary>
    string ResponseStatus { get; }

    /// <summary>
    /// TODO Documentation
    /// </summary>
    /// <typeparam name="TResult"></typeparam>
    /// <param name="route"></param>
    /// <param name="query"></param>
    /// <param name="authHeader"></param>
    /// <param name="additionalHeaders"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task<TResult?> GetAsync<TResult>(
        string route,
        HttpQuery? query = default,
        AuthenticationHeaderValue? authHeader = default,
        IEnumerable<RequestHeader>? additionalHeaders = default,
        CancellationToken cancellationToken = default) where TResult : class;
}
