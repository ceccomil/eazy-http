namespace EazyHttp.Generator;

public class SyntaxReceiver : ISyntaxContextReceiver
{
    private const string EXT_METHOD = "ConfigureEazyHttpClients";
    private const string CLIENTS_DEF = ".EazyHttpClients";
    private const string CLIENTS_PROP = "EazyHttpClients";

    public List<string> GeneratorLogger { get; } = new();
    public List<dynamic> Clients { get; private set; } = new();

    public bool ConfigFound { get; private set; }

    public void OnVisitSyntaxNode(GeneratorSyntaxContext context)
    {
        if (!ConfigFound &&
            context.Node is InvocationExpressionSyntax ies &&
            $"{ies}" is string code &&
            code.Contains(EXT_METHOD))
        {
            GeneratorLogger.Add(
                $"Search for `{EXT_METHOD}`: " +
                $"is successful!{Environment.NewLine}{Environment.NewLine}");

            GeneratorLogger.Add(
                $"Found: {code}{Environment.NewLine}");

            ConfigFound = true;

            var lambda = ies
                .DescendantNodes()
                .OfType<LambdaExpressionSyntax>()
                .FirstOrDefault(x => $"{x}".Contains(CLIENTS_DEF));

            if (lambda is null)
            {
                Clients
                    .Add(new {
                        Name = "SharedHttpClient",
                        BaseAddress = ""
                    });

                GeneratorLogger.Add(
                        $"Adding default client: only {Clients.Count} client!");

                return;
            }

            GeneratorLogger.Add(
                $"Found client config: {lambda}{Environment.NewLine}");

            if (lambda.Body is not BlockSyntax body)
            {
                throw new NotSupportedException(
                    $"Lambda body: {lambda}, is not of " +
                    $"type {nameof(BlockSyntax)}");
            }

            GetClientsSetupFromLambda(
                body);
        }
    }

    private void GetClientsSetupFromLambda(
        BlockSyntax body)
    {
        GeneratorLogger.Add(
                    $"Lambda Body: {body}");

        var nodes = body
            .DescendantNodes()
            .OfType<ExpressionStatementSyntax>()
            .Where(x => $"{x}".Contains($".{CLIENTS_PROP}"));

        var statements = new List<string>();

        foreach (var n in nodes)
        {
            GeneratorLogger.Add(
                $"Nodes {n.GetType().Name}: {n}");

            var statement = $"{n}";

            var idx = statement
                .IndexOf(CLIENTS_PROP);

            statements
                .Add(
                    statement
                    .Substring(idx));
        }

        var code = @$"
public class Definition
{{
    public string Name {{ get; }}
    public string? BaseAddress {{ get; }}

    public Definition(
    string name,
    string? baseAddress = default)
    {{
        Name = name;
        BaseAddress = baseAddress;
    }}

    public override string ToString() => $""{{Name}} ({{BaseAddress}})"";
}}

var {CLIENTS_PROP} = new List<Definition>();
{string.Join(Environment.NewLine, statements)}
return {CLIENTS_PROP};";

        var exprEval = new ExpressionEvaluator();
        var input = exprEval
            .InitInput(new()
            {
                Code = code
            });

        try
        {
            var result = exprEval
                .GetEvaluated<dynamic>(input)
                .Result;

            Clients.Clear();

            foreach (var d in result)
            {
                GeneratorLogger.Add(
                        $"Results: {d}");

                Clients.Add(d);
            }
        }
        catch (Exception ex)
        {
            GeneratorLogger.Add(
                     $"ERROR: {ex}");
        }

        GeneratorLogger.Add(
                     $"ExpressionEx: {exprEval.EvalException}");
    }
}
