namespace EazyHttp.Generator.Tests.Generators;

[Collection("HttpClientGenerator Tests")]
public class HttpClientGeneratorTests
{
  [Fact]
  public void DebuggingTest()
  {
    // Arrange
    var compilation = SyntaxTrees.CreateCompilation("./Data", false);
    var generator = new HttpClientGenerator();
    var driver = CSharpGeneratorDriver.Create(generator);

    // Act
    driver = (CSharpGeneratorDriver)driver
      .RunGeneratorsAndUpdateCompilation(
        compilation,
        out var _,
        out var diagnostics);

    var files = driver
      .GetGeneratedCode()
      .ToList();

    foreach (var st in compilation.SyntaxTrees)
    {
      files.Add(new(st.FilePath, st.ToString()));
    }

    var finalDiagnostics = files.GetDiagnosticsForGenerated(false);
    var readableDiagnostics = finalDiagnostics
      .ToString();

    // Assert
    Assert.NotEmpty(files);
    Assert.Empty(diagnostics);
    Assert.Empty(finalDiagnostics.Errors);
    Assert.NotEmpty(readableDiagnostics);
  }
}