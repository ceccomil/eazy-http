namespace EazyHttp.Contracts;

/// <summary>
/// Latest response results.
/// </summary>
public sealed class ResponseResultsCollection : ICollection<ResponseResult>
{
    private readonly List<ResponseResult> _results = [];
    private ResponseResult? _last;

    /// <summary>
    /// Last response result.
    /// </summary>
    public ResponseResult? Last => _last;

    /// <inheritdoc />
    public int Count => _results.Count;

    /// <inheritdoc />
    public bool IsReadOnly => false;

    /// <inheritdoc />
    public void Add(
        ResponseResult item)
    {
        _results.Add(item);
        _last = item;
    }

    /// <inheritdoc />
    public void Clear()
    {
        _results.Clear();
        _last = null;
    }

    /// <inheritdoc />
    public bool Contains(
        ResponseResult item) => _results
        .Contains(item);

    /// <inheritdoc />
    public void CopyTo(
        ResponseResult[] array,
        int arrayIndex) => _results
        .CopyTo(array, arrayIndex);

    /// <inheritdoc />
    public IEnumerator<ResponseResult> GetEnumerator() => _results
        .GetEnumerator();

    /// <inheritdoc />
    public bool Remove(
        ResponseResult item)
    {
        if (!_results
            .Remove(item))
        {
            return false;
        }

        _last = _results
            .LastOrDefault();

        return true;
    }

    /// <inheritdoc />
    IEnumerator IEnumerable.GetEnumerator() => _results
        .GetEnumerator();
}
