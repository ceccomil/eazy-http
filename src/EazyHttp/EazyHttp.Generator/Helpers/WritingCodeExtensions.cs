namespace EazyHttp.Generator.Helpers;

internal static class CodeWritingExtensions
{
  public static string ToNullableDirectiveIfEnabled(
    this bool nullable)
  {
    return nullable ? "#nullable enable\r\n" : "";
  }

  public static string GetFormattedCode(this SyntaxNode code)
  {
    var formattedCode = code
        .NormalizeWhitespace(indentation: "  ")
        .SyntaxTree
        .GetText()
        .ToString();

    return formattedCode;
  }

  public static string GetFormattedCode(this string code)
  {
    var formattedCode = CSharpSyntaxTree
        .ParseText(code)
        .GetRoot()
        .GetFormattedCode();

    return formattedCode;
  }

  public static string GetUsings(
    this IEnumerable<UsingDirectiveSyntax> usings,
    bool normalizeWhitespace = false)
  {
    if (normalizeWhitespace)
    {
      return string.Join("\r\n", usings.Select(x => $"{x.NormalizeWhitespace()}"));
    }

    return string.Join("\r\n", usings.Select(x => $"{x}"));
  }

  public static string GetEazyHttpClientNamespace(
    this string namespacePrefix)
  {
    var ns = $"{namespacePrefix}.{Generated}.{Clients}";

    return ns;
  }

  public static string GetEazyHttpClientFilename(
    this HttpClientDefinition definition,
    string @namespace)
  {
    var name = $"{@namespace}.{definition.Name}.g.cs";

    return name;
  }

  public static string GetDiRegistrationFilename(
    this EazyClientOptionsDefinition options)
  {
    var ns = $"{nameof(EazyHttp)}.{Generated}.{DependencyInjection}";

    var name = $"{ns}.{options.GetNameByNamespace()}.g.cs";

    return name;
  }

  public static string? TryGetHandlerTypeName(this string raw)
  {
    if (string.IsNullOrWhiteSpace(raw))
    {
      return null;
    }

    raw = raw.Trim();

    // typeof(MyType)
    const string typeofPrefix = "typeof(";
    if (raw.StartsWith(typeofPrefix, StringComparison.Ordinal) &&
      raw.EndsWith(")", StringComparison.Ordinal))
    {
      var inner = raw.Substring(
        typeofPrefix.Length, 
        raw.Length - typeofPrefix.Length - 1);
      
      return inner.Trim();
    }

    return raw;
  }

  public static string GetNameByNamespace(
    this EazyClientOptionsDefinition options)
  {
    var sb = new StringBuilder();
    foreach (var ch in options.NamespacePrefix)
    {
      if (char.IsLetterOrDigit(ch))
      {
        sb.Append(ch);
      }
    }

    var safe = sb.Length > 0 
      ? sb.ToString() 
      : nameof(EazyHttp);

    return $"Add{safe}Clients";
  }
}
