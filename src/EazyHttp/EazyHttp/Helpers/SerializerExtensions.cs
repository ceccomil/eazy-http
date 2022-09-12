namespace EazyHttp.Helpers;

internal static class SerializerExtensions
{
    internal static async Task<HttpContent?> GetContentFromBody(
        this JsonSerializerOptions options,
        object? body,
        Encoding enc,
        CancellationToken cancellationToken = default)
    {
        if (body is null)
        {
            return default;
        }

        using var stream = new MemoryStream();
        await JsonSerializer
            .SerializeAsync(
                stream,
                body,
                body.GetType(),
                options,
                cancellationToken);

        stream.Seek(
            0,
            SeekOrigin.Begin);

        var buffer = stream
            .ToArray();

        return new StringContent(
            enc.GetString(
                buffer),
            enc,
            "application/json");
    }
}
