namespace EazyHttp.Generator.Tests.Generators;

[Collection("DiRegistrationGenerator Tests")]
public class DiRegistrationGeneratorTests
{
  [Fact]
  public void DebuggingTest()
  {
    // Arrange
    var compilation = SyntaxTrees.CreateCompilation("./Data");
    var generator = new DiRegistrationGenerator();
    var clientsGenerator = new HttpClientGenerator();
    var driver = CSharpGeneratorDriver.Create(generator, clientsGenerator);

    // Act
    driver = (CSharpGeneratorDriver)driver
      .RunGeneratorsAndUpdateCompilation(
        compilation,
        out var outputCompilation,
        out var diagnostics);

    var files = driver
      .GetGeneratedCode()
      .ToList();

    foreach (var st in compilation.SyntaxTrees)
    {
      files.Add(new(st.FilePath, st.ToString()));
    }

    files.Add(new("usings.cs", "global using Tests.Data;"));

    var finalDiagnostics = files.GetDiagnosticsForGenerated();
    //var readableDiagnostics = finalDiagnostics
    //  .ToString();

    // Assert
    Assert.NotEmpty(files);
    Assert.Empty(finalDiagnostics.Errors);
  }

  [Fact]
  public void NoOptionsGeneratesSharedClient()
  {
    // Arrange
    var compilation = SyntaxTrees.CreateCompilation("./Data/NoOptions");
    var generator = new DiRegistrationGenerator();
    var clientsGenerator = new HttpClientGenerator();
    var driver = CSharpGeneratorDriver.Create(generator, clientsGenerator);

    // Act
    driver = (CSharpGeneratorDriver)driver
      .RunGeneratorsAndUpdateCompilation(
        compilation,
        out var outputCompilation,
        out var diagnostics);

    var files = driver
      .GetGeneratedCode()
      .ToList();

    foreach (var st in compilation.SyntaxTrees)
    {
      files.Add(new(st.FilePath, st.ToString()));
    }

    files.Add(new("usings.cs", "global using Tests.Data;"));

    var finalDiagnostics = files.GetDiagnosticsForGenerated();
    //var readableDiagnostics = finalDiagnostics
    //  .ToString();

    // Assert
    Assert.NotEmpty(files);
    Assert.Empty(finalDiagnostics.Errors);
  }
}
