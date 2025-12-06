namespace EazyHttp.Contracts;

/// <summary>
/// Represents the HttpRequest query string collection.
/// </summary>
public record HttpQuery
{
  private readonly List<HttpQueryParam> _queryParameters = new();

  /// <summary>
  /// The collection of query parameters.
  /// </summary>
  public IEnumerable<HttpQueryParam> Parameters
  {
    get => _queryParameters;
  }

  /// <summary>
  /// Append a new <see cref="HttpQueryParam"/> to the collection.
  /// </summary>
  public void AddParam(
      HttpQueryParam parameter) => _queryParameters
      .Add(parameter);

  /// <summary>
  /// Returns the URL encoded query string.
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
/// Represents an http query field with its values
/// </summary>
public record HttpQueryParam
{
  /// <summary>
  /// Gets the value of the field represented by this property.
  /// </summary>
  public string Field { get; }

  /// <summary>
  /// Gets the collection of values associated with the query field.
  /// </summary>
  public string?[] Values { get; }

  /// <summary>
  /// Initializes a new instance of the HttpQueryParam class with the specified field name and associated values.
  /// </summary>
  /// <param name="field">The name of the query parameter field. Cannot be null.</param>
  /// <param name="values">One or more values to associate with the query parameter. Null values are allowed and will be included as-is.</param>
  public HttpQueryParam(
    string field,
    params string?[] values)
  {
    values ??= [];

    Field = field;
    Values = values;
  }
}
