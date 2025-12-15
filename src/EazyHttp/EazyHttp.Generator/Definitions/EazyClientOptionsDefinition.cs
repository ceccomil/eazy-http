namespace EazyHttp.Generator.Definitions;

internal sealed class EazyClientOptionsDefinition
{
  public string NamespacePrefix { get; set; } = nameof(EazyHttp);

  public List<HttpClientDefinition> Clients { get; } = [];

  public Dictionary<string, string> RequestSerializerOptions { get; } = [];

  public Dictionary<string, string> ResponseSerializerOptions { get; } = [];

  public Dictionary<string, string> Retries { get; } = [];

  public Dictionary<string, string> Encodings { get; } = [];

  public Dictionary<string, string> PersistentHeaders { get; } = [];

  public Dictionary<string, HandlerDefinition> HttpClientHandlers { get; } = [];
}
