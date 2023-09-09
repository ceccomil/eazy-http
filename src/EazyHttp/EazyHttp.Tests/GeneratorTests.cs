namespace EazyHttp.Tests;

[Collection("Generator Tests")]
public class GeneratorTests
{
    private static Compilation CreateCompilation(string source)
            => CSharpCompilation.Create("compilation",
                new[]
                {
                    CSharpSyntaxTree.ParseText(source)
                },
                new[]
                {
                    MetadataReference
                    .CreateFromFile(
                        typeof(Binder)
                        .GetTypeInfo()
                        .Assembly
                        .Location)
                },
                new CSharpCompilationOptions(
                    OutputKind
                    .ConsoleApplication));

    [Theory]
    //[InlineData("""
    //    services
    //    .ConfigureEazyHttpClients(opts =>
    //    {
    //        opts
    //            .NameSpacePrefix = 
    //                "DataDog";

    //        opts
    //            .EazyHttpClients
    //            .Add(new(
    //                "DataDogLogs",
    //                Gpm
    //                .Core
    //                .Logging
    //                .LoggerExtensions
    //                .DataDogIntakeUrl));

    //        opts
    //            .PersistentHeaders
    //            .Add(
    //                "DataDogLogs",
    //                new RequestHeader[]
    //                {
    //            new(
    //                HeaderNames.Accept,
    //                "application/json"),
    //            new(
    //                "DD-API-KEY",
    //                dataDogApiKey)
    //                });

    //        opts
    //            .SerializersOptions
    //            .Add(
    //                "DataDogLogs",
    //                new(
    //                    JsonSerializerDefaults.Web)
    //                {
    //                    PropertyNamingPolicy = JsonNamingPolicy
    //                        .CamelCase,

    //                    WriteIndented = false,

    //                    DefaultIgnoreCondition = JsonIgnoreCondition
    //                        .WhenWritingNull
    //                });

    //        opts
    //            .Retries
    //            .Add(
    //                "DataDogLogs",
    //                new()
    //                {
    //                    MaxAttempts = 4,
    //                    StatusCodeMatchingCondition = (code, method) =>
    //                    {
    //                        if (method != HttpMethod.Post)
    //                        {
    //                            return false;
    //                        }

    //                        if (code is HttpStatusCode.ServiceUnavailable
    //                            or HttpStatusCode.GatewayTimeout
    //                            or HttpStatusCode.RequestTimeout)
    //                        {
    //                            return true;
    //                        }

    //                        return false;
    //                    }
    //                });
    //    })
    //    """)]
    [InlineData("""
        services
        .ConfigureEazyHttpClients(opts =>
        {        
            opts
                .EazyHttpClients.Add(new HttpClientDefinition(
                    "DataDogLogs",
                    "https://http-intake.logs.datadoghq.eu/api/v2"));
        
        })
        """)]
    [InlineData("""
        services
        .ConfigureEazyHttpClients(opts =>
        {        
            opts
                .EazyHttpClients.Add(new(
                    "DataDogLogs",
                    "https://http-intake.logs.datadoghq.eu/api/v2"));
        
        })
        """)]
    [InlineData("""
        services
        .ConfigureEazyHttpClients(opts =>
        {        
            opts
                .EazyHttpClients.Add(new HttpClientDefinition(
                    "NoBaseAddress"));
        
        })
        """)]
    public void Generator_when_is_expected_to_work(
        string code)
    {
        // Arrange
        var inputClass = CreateCompilation(
            code);

        var codeGen = new IncrementalGenerator();
        var driver = CSharpGeneratorDriver
            .Create(codeGen);

        // Act
        driver = (CSharpGeneratorDriver)driver
            .RunGeneratorsAndUpdateCompilation(
            inputClass,
            out var outputCompilation,
            out var diagnostics);

        var results = driver
            .GetRunResult()
            .GeneratedTrees;

        var codes = new List<string>();

        foreach (var r in results)
        {
            codes.Add($"{r}");
        }

        // Assert
        diagnostics
            .Should()
            .BeEmpty();

        outputCompilation
            .SyntaxTrees
            .Should()
            .NotBeEmpty();
    }
}
