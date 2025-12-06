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
    var name = $"{options.NamespacePrefix}.DiRegistration.g.cs";

    return name;
  }

  public static string? TryGetHandlerTypeName(this string raw)
  {
    if (string.IsNullOrWhiteSpace(raw))
    {
      return null;
    }

    raw = raw.Trim();

    // nameof(MyType)
    const string nameofPrefix = "nameof(";
    if (raw.StartsWith(nameofPrefix, StringComparison.Ordinal) &&
      raw.EndsWith(")", StringComparison.Ordinal))
    {
      var inner = raw.Substring(nameofPrefix.Length, raw.Length - nameofPrefix.Length - 1);
      return inner.Trim(); // "MyType" (or "My.Namespace.MyType" if they wrote it)
    }

    // "MyType"
    if (raw.Length >= 2 && raw[0] == '"' && raw[raw.Length - 1] == '"')
    {
      return raw.Substring(1, raw.Length - 2); // strip quotes
    }

    // Fallback: treat as already a type name expression
    // e.g. CustomHandler, MyApp.Http.CustomHandler, global::MyApp.Http.CustomHandler
    return raw;
  }
}
