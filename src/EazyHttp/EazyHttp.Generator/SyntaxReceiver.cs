namespace EazyHttp.Generator;

public class SyntaxReceiver : ISyntaxContextReceiver
{
    private const string EXT_METHOD = "ConfigureEazyHttpClients";
    private const string CLIENTS_DEF = ".EazyHttpClients";
    private const string CLIENTS_PROP = "EazyHttpClients";
    private const string NAMESPACE_PREFIX_DEF = ".NameSpacePrefix";
    private const string NAMESPACE_PREFIX_PROP = "NameSpacePrefix";
    private const string HANDLERS_DEF = ".HttpClientHandlers";
    private const string HANDLERS_PROP = "HttpClientHandlers";

    public List<string> GeneratorLogger { get; } = new();
    public List<dynamic> Clients { get; } = new();
    public Dictionary<string, string> Handlers { get; } = new();

    public bool ConfigFound { get; private set; }

    private string _nameSpacePrefix = string.Empty;
    
    public string NameSpacePrefix
    {
        get
        {
            if (string.IsNullOrWhiteSpace(_nameSpacePrefix))
            {
                return string.Empty;
            }

            while (_nameSpacePrefix.EndsWith("."))
            {
                _nameSpacePrefix = _nameSpacePrefix
                    .Remove(
                        _nameSpacePrefix
                        .Length - 1,
                        1);
            }

            return $"{_nameSpacePrefix}.";
        }
    }

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

            GetClientsSetup(
                ies);

            SetNameSpacePrefix(
                ies);

            GetHandlers(
                ies);
        }
    }

    private void SetNameSpacePrefix(
        InvocationExpressionSyntax ies)
    {
        var lambda = ies
                .DescendantNodes()
                .OfType<LambdaExpressionSyntax>()
                .LastOrDefault(x => $"{x}".Contains(NAMESPACE_PREFIX_DEF));

        if (lambda is null)
        {
            return;
        }

        GeneratorLogger.Add(
            $"Search for `{NAMESPACE_PREFIX_DEF}`: " +
            $"is successful!{Environment.NewLine}{Environment.NewLine}");

        if (lambda.Body is not BlockSyntax body)
        {
            throw new NotSupportedException(
                $"Lambda body: {lambda}, is not of " +
                $"type {nameof(BlockSyntax)}");
        }

        SetNameSpacePrefixFromLambda(
            body);
    }

    private void SetNameSpacePrefixFromLambda(
        BlockSyntax body)
    {
        GeneratorLogger.Add(
                    $"Lambda Body: {body}");

        var nodes = body
            .DescendantNodes()
            .OfType<ExpressionStatementSyntax>()
            .Where(x => $"{x}".Contains(NAMESPACE_PREFIX_DEF));

        var statements = new List<string>();

        foreach (var n in nodes)
        {
            GeneratorLogger.Add(
                $"Nodes {n.GetType().Name}: {n}");

            var statement = n
                .FlattenLine();

            var idx = statement
                .IndexOf(NAMESPACE_PREFIX_PROP);

            statements
                .Add(
                    statement
                    .Substring(idx));
        }

        var code = @$"
var {string.Join(Environment.NewLine, statements)}

return {NAMESPACE_PREFIX_PROP};";

        var exprEval = new ExpressionEvaluator();
        var input = exprEval
            .InitInput(new()
            {
                Code = code
            });

        try
        {
            var result = exprEval
                .GetEvaluated<string>(input)
                .Result;

            _nameSpacePrefix = $"{result}";
        }
        catch (Exception ex)
        {
            GeneratorLogger.Add(
                     $"ERROR: {ex}");

            GeneratorLogger.Add(
                     $"Source code:{Environment.NewLine}" +
                     $"{code}{Environment.NewLine}{Environment.NewLine}");
        }

        if (exprEval.EvalException is not null)
        {
            GeneratorLogger.Add(
                         $"ExpressionEx: {exprEval.EvalException}");

            GeneratorLogger.Add(
                     $"Source code:{Environment.NewLine}" +
                     $"{code}{Environment.NewLine}{Environment.NewLine}");
        }

        GeneratorLogger.Add(
                $"NameSpacePrefix: {NameSpacePrefix}");
    }

    private void GetClientsSetup(
        InvocationExpressionSyntax ies)
    {
        var lambda = ies
                .DescendantNodes()
                .OfType<LambdaExpressionSyntax>()
                .LastOrDefault(x => $"{x}".Contains(CLIENTS_DEF));

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

    private void GetClientsSetupFromLambda(
        BlockSyntax body)
    {
        GeneratorLogger.Add(
                    $"Lambda Body: {body}");

        var nodes = body
            .DescendantNodes()
            .OfType<ExpressionStatementSyntax>()
            .Where(x => $"{x}".Contains(CLIENTS_DEF));

        var statements = new List<string>();

        foreach (var n in nodes)
        {
            GeneratorLogger.Add(
                $"Nodes {n.GetType().Name}: {n}");

            var statement = n
                .ToVariableStatement();

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

        GeneratorLogger.Add(
                $"Code to be evaluated: {code}");

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

        if (exprEval.EvalException is not null)
        {
            GeneratorLogger.Add(
                         $"ExpressionEx: {exprEval.EvalException}");

            GeneratorLogger.Add(
                     $"Source code:{Environment.NewLine}" +
                     $"{code}{Environment.NewLine}{Environment.NewLine}");
        }
    }

    private void GetHandlers(
        InvocationExpressionSyntax ies)
    {
        var lambda = ies
                .DescendantNodes()
                .OfType<LambdaExpressionSyntax>()
                .LastOrDefault(x => $"{x}".Contains(HANDLERS_DEF));

        if (lambda is null)
        {
            return;
        }

        GeneratorLogger.Add(
            $"Found handlers config: {lambda}{Environment.NewLine}");

        if (lambda.Body is not BlockSyntax body)
        {
            throw new NotSupportedException(
                $"Lambda body: {lambda}, is not of " +
                $"type {nameof(BlockSyntax)}");
        }

        GetHandlersFromLambda(
            body);
    }

    private void GetHandlersFromLambda(
        BlockSyntax body)
    {
        GeneratorLogger.Add(
                    $"Lambda Body: {body}");

        var nodes = body
            .DescendantNodes()
            .OfType<ExpressionStatementSyntax>()
            .Where(x => $"{x}".Contains(HANDLERS_DEF));

        foreach (var n in nodes)
        {
            GeneratorLogger.Add(
                $"Nodes {n.GetType().Name}: {n}");

            var statement = n
                .FlattenLine();

            GeneratorLogger.Add(
                $"Stripped : {statement}");

            var match = StringsPair
                .Match(statement);

            if (!match.Success)
            {
                continue;
            }

            if (Clients.Any(x => x.Name == match.Groups[1].Value) &&
                !Handlers.ContainsKey(match.Groups[1].Value))
            {
                Handlers.Add(
                    match.Groups[1].Value,
                    match.Groups[2].Value);
            }
        }

        GeneratorLogger.Add(
                $"Handlers found {Handlers.Count}");
    }
}
