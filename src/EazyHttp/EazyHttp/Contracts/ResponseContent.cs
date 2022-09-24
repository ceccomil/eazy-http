namespace EazyHttp.Contracts;

internal class ResponseContent : IDisposable
{
    public HttpStatusCode StatusCode => _response.StatusCode;
    public Stream Content { get; }
    public HttpContentHeaders Headers { get; }
    public bool IsSuccess => _response.IsSuccessStatusCode;

    private bool _disposed;
    private readonly HttpResponseMessage _response;

    public ResponseContent(
        HttpResponseMessage response,
        Stream content)
    {
        _response = response;
        Content = content;
        Headers = response
            .Content
            .Headers;
    }

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

