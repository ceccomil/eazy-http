namespace EazyHttp.Generator.Tests.Helpers;

internal static class SyntaxTrees
{
  public static List<SyntaxTree> GetTrees(string sourceFolder)
  {
    List<SyntaxTree> trees = [];

    foreach (var file in Directory.EnumerateFiles(
      sourceFolder, "*.cs", SearchOption.AllDirectories))
    {
      var content = File.ReadAllText(file);

      var relativePath = Path
        .GetRelativePath(sourceFolder, file)
        .Replace("\\", "/");

      var tree = CSharpSyntaxTree.ParseText(content, path: relativePath);

      trees.Add(tree);
    }
    return trees;
  }

  public static CSharpCompilation CreateCompilation(
    string sourceFolder,
    bool enableNullable = false)
  {
    var trees = GetTrees(sourceFolder);

    var nullable = enableNullable
      ? NullableContextOptions.Enable
      : NullableContextOptions.Disable;

    var options = new CSharpCompilationOptions(
      OutputKind.ConsoleApplication,
      nullableContextOptions: nullable);

    return CSharpCompilation.Create(
      "compilation",
      trees,
      CustomDiagnostics.GetMetadataReferences(),
      options);
  }
}
