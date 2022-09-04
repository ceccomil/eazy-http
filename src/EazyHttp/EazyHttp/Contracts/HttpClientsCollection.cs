namespace EazyHttp.Contracts;

/// <summary>
/// TODO documentation
/// </summary>
public class HttpClientsCollection : ICollection<HttpClientDefinition>
{
    private readonly List<HttpClientDefinition> _clients = new();

    /// <summary>
    /// TODO documentation
    /// </summary>
    public int Count => _clients
        .Count;

    /// <summary>
    /// TODO documentation
    /// </summary>
    public bool IsReadOnly => false;

    /// <summary>
    /// TODO documentation
    /// </summary>
    public HttpClientsCollection(
        IEnumerable<HttpClientDefinition>? collection = default)
    {
        if (collection is null ||
            !collection.Any())
        {
            return;
        }

        _clients
            .AddRange(collection);
    }

    /// <summary>
    /// TODO documentation
    /// </summary>
    public void Add(
        HttpClientDefinition item) => _clients
            .Add(item);

    /// <summary>
    /// TODO documentation
    /// </summary>
    public void Clear() => _clients
        .Clear();

    /// <summary>
    /// TODO documentation
    /// </summary>
    public bool Contains(
        HttpClientDefinition item) => _clients
            .Contains(item);

    /// <summary>
    /// TODO documentation
    /// </summary>
    public void CopyTo(
        HttpClientDefinition[] array,
        int arrayIndex) => _clients
            .CopyTo(array, arrayIndex);

    /// <summary>
    /// TODO documentation
    /// </summary>
    public IEnumerator<HttpClientDefinition> GetEnumerator() => _clients
        .GetEnumerator();

    /// <summary>
    /// TODO documentation
    /// </summary>
    public bool Remove(
        HttpClientDefinition item) => _clients
            .Remove(item);

    IEnumerator IEnumerable.GetEnumerator() => _clients
        .GetEnumerator();

    /// <summary>
    /// TODO documentation
    /// </summary>
    /// <returns></returns>
    public override string ToString() => string
        .Join(Environment.NewLine, _clients);
}

/// <summary>
/// TODO documentation
/// </summary>
public class HttpClientDefinition
{
    /// <summary>
    /// Http client name
    /// </summary>
    public string Name { get; }

    /// <summary>
    /// Http client base address
    /// </summary>
    public string? BaseAddress { get; }

    /// <summary>
    /// TODO documentati
    /// </summary>
    /// <param name="name"></param>
    /// <param name="baseAddress"></param>
    public HttpClientDefinition(
    string name,
    string? baseAddress = default)
    {
        Name = name;
        BaseAddress = baseAddress;
    }

    /// <summary>
    /// TODO docum
    /// </summary>
    /// <returns></returns>
    public override string ToString() => $"{Name} ({BaseAddress})";
}