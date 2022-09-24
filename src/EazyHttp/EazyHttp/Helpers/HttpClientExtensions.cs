using System.Net.Http;

namespace EazyHttp.Helpers;

internal static class HttpClientExtensions
{
    public static void AddHeaders(
        this HttpClient httpClient,
        IEnumerable<RequestHeader>? headers)
    {
        if (headers is null)
        {
            return;
        }

        foreach (var h in headers.Distinct())
        {
            httpClient
                .DefaultRequestHeaders
                .Add(
                    h.Key,
                    h.Value);
        }
    }

    public static void RemoveHeaders(
        this HttpClient httpClient,
        IEnumerable<RequestHeader>? headers)
    {
        if (headers is null)
        {
            return;
        }

        foreach (var h in headers.Distinct())
        {
            httpClient
                .DefaultRequestHeaders
                .Remove(
                    h.Key);
        }
    }
}
