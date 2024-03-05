namespace EazyHttp.Contracts;

internal class ResponseContent(
    HttpResponseMessage _response,
    Stream _content) : IDisposable
{
    public HttpStatusCode StatusCode => _response.StatusCode;
    public Stream Content { get; } = _content;

    public HttpContentHeaders ContentHeaders { get; } = _response
            .Content
            .Headers;

    public HttpResponseHeaders Headers { get; } = _response
        .Headers;

    public bool IsSuccess => _response.IsSuccessStatusCode;

    private bool _disposed;

    ~ResponseContent() => Dispose(false);

    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    private void Dispose(
        bool disposing)
    {
        if (_disposed)
        {
            return;
        }

        _disposed = true;

        if (disposing)
        {
            Content.Close();
            Content.Dispose();

            _response.Dispose();
        }
    }
}

