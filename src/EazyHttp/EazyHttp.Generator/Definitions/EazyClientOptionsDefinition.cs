namespace EazyHttp.Generator.Definitions;

internal sealed class EazyClientOptionsDefinition
{
  public string NamespacePrefix { get; set; } = nameof(EazyHttp);

  public List<HttpClientDefinition> Clients { get; } = [];

  public Dictionary<string, string> SerializerOptions { get; } = [];

  public Dictionary<string, string> Retries { get; } = [];

  public Dictionary<string, string> Encodings { get; } = [];

  public Dictionary<string, string> PersistentHeaders { get; } = [];

  public Dictionary<string, string> HttpClientHandlers { get; } = [];

  public string? ResolveRetryExpression { get; set; }

  public string? ResolveEncodingExpression { get; set; }
  
  public string? ResolveHeadersExpression { get; set; }
}
