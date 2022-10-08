namespace EazyHttp.Contracts;

/// <summary>
/// Represent the collection of <see cref="IEazyHttpClient"/> definitions.
/// </summary>
public class HttpClientsCollection : ICollection<HttpClientDefinition>
{
    private readonly List<HttpClientDefinition> _clients = new();

    /// <inheritdoc/>
    public int Count => _clients
        .Count;

    /// <inheritdoc/>
    public bool IsReadOnly => false;

    /// <summary>
    /// Default constructor.
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

    /// <inheritdoc/>
    public void Add(
        HttpClientDefinition item) => _clients
            .Add(item);

    /// <inheritdoc/>
    public void Clear() => _clients
        .Clear();

    /// <inheritdoc/>
    public bool Contains(
        HttpClientDefinition item) => _clients
            .Contains(item);

    /// <inheritdoc/>
    public void CopyTo(
        HttpClientDefinition[] array,
        int arrayIndex) => _clients
            .CopyTo(array, arrayIndex);

    /// <inheritdoc/>
    public IEnumerator<HttpClientDefinition> GetEnumerator() => _clients
        .GetEnumerator();

    /// <inheritdoc/>
    public bool Remove(
        HttpClientDefinition item) => _clients
            .Remove(item);

    IEnumerator IEnumerable.GetEnumerator() => _clients
        .GetEnumerator();

    /// <inheritdoc/>
    public override string ToString() => string
        .Join(Environment.NewLine, _clients);
}

/// <summary>
/// Represent the definition used to create an <see cref="IEazyHttpClient"/>.
/// Client name and base address.
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
    /// Default constructor
    /// </summary>
    public HttpClientDefinition(
    string name,
    string? baseAddress = default)
    {
        Name = name;
        BaseAddress = baseAddress;
    }

    /// <inheritdoc/>
    public override string ToString() => $"{Name} ({BaseAddress})";
}