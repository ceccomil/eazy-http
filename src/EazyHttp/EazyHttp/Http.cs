namespace EazyHttp.HttpClients;

internal class Http : EazyHttpClientBase
{
    public Http(
        HttpClient httpClient,
        IOptions<EazyClientOptions> options)
        : base(httpClient, options)
    {
        if (!string.IsNullOrWhiteSpace(""))
        {
            httpClient.BaseAddress = new Uri("whatev");
        }
    }
}
