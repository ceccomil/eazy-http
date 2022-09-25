namespace EazyHttp.Contracts;

internal class Jitter
{
    public List<FailedRequestException> FailedRequests { get; } = new();
    public TimeSpan LastWait { get; set; }
}
