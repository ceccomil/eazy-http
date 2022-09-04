namespace EazyHttp.Contracts;

/// <summary>
/// TODO documentation
/// </summary>
public record HttpQuery
{
    private readonly List<HttpQueryParam> _queryParameters = new();

    /// <summary>
    /// TODO documentation
    /// </summary>
    public IEnumerable<HttpQueryParam> Parameters {
        get => _queryParameters;
    }

    /// <summary>
    /// TODO documentation
    /// </summary>
    /// <param name="parameter"></param>
    public void AddParam(
        HttpQueryParam parameter) => _queryParameters
        .Add(parameter);

    /// <summary>
    /// Returns the URL encoded query string
    /// </summary>
    public override string ToString()
    {
        if (!Parameters.Any())
        {
            return string.Empty;
        }

        var query = "?";

        foreach (var p in Parameters)
        {
            var values = p
                .Values
                .Where(x => !string.IsNullOrWhiteSpace(x))
                .Distinct();

            var field = HttpUtility
                .UrlEncode(p.Field);

            foreach (var v in values)
            {
                var value = HttpUtility
                    .UrlEncode(v);

                query += $"{field}={value}&";
            }
        }

        return query
            .Remove(query.Length - 1);
    }
}

/// <summary>
/// TODO documentation
/// </summary>
/// <param name="Field"></param>
/// <param name="Values"></param>
public record HttpQueryParam(
    string Field,
    params string[] Values);
