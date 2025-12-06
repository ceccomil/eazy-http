namespace EazyHttp.Generator.Tests.Helpers;

internal static class CustomDiagnostics
{
  public static EnrichedDiagnostics GetDiagnosticsForGenerated(
    this IEnumerable<GeneratedFile> generated,
    bool enableNullable = false)
  {
    var refs = GetMetadataReferences();

    var nullable = enableNullable
      ? NullableContextOptions.Enable
      : NullableContextOptions.Disable;

    List<SyntaxTree> trees = [];

    foreach (var g in generated)
    {
      trees.Add(CSharpSyntaxTree.ParseText(g.Content, path: g.Name));
    }

    var options = new CSharpCompilationOptions(
      OutputKind.DynamicallyLinkedLibrary,
      nullableContextOptions: nullable);

    var compilation = CSharpCompilation.Create(
        "resulting_compilation",
        trees,
        refs,
        options);

    var diags = new EnrichedDiagnostics(compilation);

    return diags;
  }

  public static IEnumerable<GeneratedFile> GetGeneratedCode(this CSharpGeneratorDriver driver)
  {
    var results = driver
      .GetRunResult()
      .GeneratedTrees;

    foreach (var result in results)
    {
      yield return Get(result);
    }
  }

  public static List<PortableExecutableReference> GetMetadataReferences()
  {
    var refs = new List<PortableExecutableReference>();

    // Making sure that reference includes EazyHttp which is not
    // directly referenced by the generator project.
    var assembly = typeof(Runtime.HttpRetryExecutor)
      .GetTypeInfo()
      .Assembly;

    var context = DependencyContext.Default
      ?? DependencyContext.Load(Assembly.GetEntryAssembly()!)
      ?? throw new InvalidOperationException("Cannot load dependency context for the entry assembly.");

    var libraries = context.RuntimeLibraries
      .SelectMany(x => x.GetDefaultAssemblyNames(context))
      .Select(Assembly.Load)
      .Select(x => x.Location)
      .Distinct()
      .ToList();

    foreach (var library in libraries)
    {
      if (refs.Any(x => x.FilePath == library))
      {
        continue;
      }

      refs.Add(MetadataReference.CreateFromFile(library));
    }

    return refs;
  }

  private static GeneratedFile Get(SyntaxTree generated)
  {
    var content = generated
      .GetRoot()
      .ToFullString();

    return new(generated.FilePath, content);
  }
}
