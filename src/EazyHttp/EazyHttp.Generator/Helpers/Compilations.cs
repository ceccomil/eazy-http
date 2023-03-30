namespace EazyHttp.Generator.Helpers;

internal static class Compilations
{
    public const string EXT_METHOD = "ConfigureEazyHttpClients";
    public const string CLIENTS_DEF = ".EazyHttpClients";
    public const string CLIENTS_PROP = "EazyHttpClients";
    public const string NAMESPACE_PREFIX_DEF = ".NameSpacePrefix";
    public const string NAMESPACE_PREFIX_PROP = "NameSpacePrefix";
    public const string HANDLERS_DEF = ".HttpClientHandlers";
    public const string HANDLERS_PROP = "HttpClientHandlers";

    private static Compilation CreateCompilation(
        this string source) =>
        CSharpCompilation
        .Create(
            "compilation",
            new[]
            {
                CSharpSyntaxTree.ParseText(source)
            },
            new[]
            {
                MetadataReference
                .CreateFromFile(
                    typeof(object)
                    .GetTypeInfo()
                    .Assembly
                    .Location)
            },
            new CSharpCompilationOptions(
                OutputKind
                .DynamicallyLinkedLibrary));

    internal static string ToNameSpacePrefix(
        this ICollection<string> statements)
    {
        var guid = Guid
            .NewGuid()
            .ToString()
            .Replace(
                "-",
                "");

        var code =
            $$"""
            public static class NameSpace_{{guid}}
            {
                public static string Get()
                {
                    var {{string.Join("\r\n", statements)}}

                    return {{NAMESPACE_PREFIX_PROP}};
                }
            }
            """;

        var compilation = CreateCompilation(
            code);

        using var ms = new MemoryStream();

        var emitResult = compilation
            .Emit(ms);

        if (!emitResult.Success)
        {
            throw new ApplicationException(
                string
                .Join(
                    "\r\n",
                    emitResult.Diagnostics));
        }

        var assembly = Assembly
            .Load(ms.ToArray());

        var type = assembly
            .GetType($"NameSpace_{guid}");

        var method = type.GetMethod(
            "Get");

        var result = method.Invoke(
            null, 
            Array.Empty<object>());

        return $"{result}";
    }

    internal static IEnumerable<dynamic> ToClientsDefinition(
        this ICollection<string> statements)
    {
        var guid = Guid
            .NewGuid()
            .ToString()
            .Replace(
                "-",
                "");

        var code = 
            $$"""
            using System;
            using System.Collections.Generic;

            public static class Definitions_{{guid}}
            {
                public class Definition
                {
                    public string Name { get; }
                    public string BaseAddress { get; }

                    public Definition(
                    string name,
                    string baseAddress)
                    {
                        Name = name;
                        BaseAddress = baseAddress;
                    }

                    public override string ToString() => $"{Name} ({BaseAddress})";
                }

                public static List<Definition> GetAll()
                {
                    var {{CLIENTS_PROP}} = new List<Definition>();
                    {{string.Join("\r\n", statements)}}
                    return {{CLIENTS_PROP}};
                }
            }
            """;

        var compilation = CreateCompilation(
            code);

        using var ms = new MemoryStream();

        var emitResult = compilation
            .Emit(ms);

        if (!emitResult.Success)
        {
            throw new ApplicationException(
                string
                .Join(
                    "\r\n",
                    emitResult.Diagnostics));
        }

        var assembly = Assembly
            .Load(ms.ToArray());

        var type = assembly
            .GetType($"Definitions_{guid}");

        var method = type.GetMethod(
            "GetAll");

        var result = method.Invoke(
            null,
            Array.Empty<object>()) 
            as IEnumerable<dynamic>;

        return result!;
    }
}