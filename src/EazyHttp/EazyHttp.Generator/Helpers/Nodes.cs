namespace EazyHttp.Generator.Helpers;

internal static class Nodes
{ 
    public static bool IsConfigFound(
        SyntaxNode node) => node is InvocationExpressionSyntax ies &&
            $"{ies}" is string code &&
            code.Contains(EXT_METHOD);

    public static Config GetConfig(
        GeneratorSyntaxContext context)
    {
        var config = new Config();
        var ies = (InvocationExpressionSyntax)context
            .Node;

        GetClientsSetup(
            ies,
            config);

        SetNameSpacePrefix(
            ies,
            config);

        GetHandlers(
            ies,
            config);

        return config;
    }

    private static void GetClientsSetup(
        InvocationExpressionSyntax ies,
        Config config)
    {
        var lambda = ies
                .DescendantNodes()
                .OfType<LambdaExpressionSyntax>()
                .LastOrDefault(x => $"{x}".Contains(CLIENTS_DEF));

        if (lambda is null)
        {
            config
            .Clients
            .Add(new {
                Name = "SharedHttpClient",
                BaseAddress = ""
            });

            return;
        }

        if (lambda.Body is not BlockSyntax body)
        {
            throw new NotSupportedException(
                $"Lambda body: {lambda}, is not of " +
                $"type {nameof(BlockSyntax)}");
        }

        GetClientsSetupFromLambda(
            body,
            config);
    }

    private static void SetNameSpacePrefix(
        InvocationExpressionSyntax ies,
        Config config)
    {
        var lambda = ies
                .DescendantNodes()
                .OfType<LambdaExpressionSyntax>()
                .LastOrDefault(x => $"{x}".Contains(NAMESPACE_PREFIX_DEF));

        if (lambda is null)
        {
            return;
        }

        if (lambda.Body is not BlockSyntax body)
        {
            throw new NotSupportedException(
                $"Lambda body: {lambda}, is not of " +
                $"type {nameof(BlockSyntax)}");
        }

        SetNameSpacePrefixFromLambda(
            body,
            config);
    }

    private static void SetNameSpacePrefixFromLambda(
        BlockSyntax body,
        Config config)
    {
        var nodes = body
            .DescendantNodes()
            .OfType<ExpressionStatementSyntax>()
            .Where(x => $"{x}".Contains(NAMESPACE_PREFIX_DEF));

        var statements = new List<string>();

        foreach (var n in nodes)
        {
            var statement = n
                .FlattenLine();

            var idx = statement
                .IndexOf(NAMESPACE_PREFIX_PROP);

            statements
                .Add(
                    statement
                    .Substring(idx));
        }

        config
            .SetNameSpacePrefix(
                statements
                .ToNameSpacePrefix());
    }

    private static void GetClientsSetupFromLambda(
        BlockSyntax body,
        Config config)
    {
        var nodes = body
            .DescendantNodes()
            .OfType<ExpressionStatementSyntax>()
            .Where(x => $"{x}".Contains(CLIENTS_DEF));

        var statements = new List<string>();

        foreach (var n in nodes)
        {
            var statement = n
                .ToVariableStatement();

            var idx = statement
                .IndexOf(CLIENTS_PROP);

            statements
                .Add(
                    statement
                    .Substring(idx));
        }

        config
            .Clients
            .Clear();

        foreach (var d in statements.ToClientsDefinition())
        {
            config
                .Clients
                .Add(d);
        }
    }

    private static void GetHandlers(
        InvocationExpressionSyntax ies,
        Config config)
    {
        var lambda = ies
                .DescendantNodes()
                .OfType<LambdaExpressionSyntax>()
                .LastOrDefault(x => $"{x}".Contains(HANDLERS_DEF));

        if (lambda is null)
        {
            return;
        }

        if (lambda.Body is not BlockSyntax body)
        {
            throw new NotSupportedException(
                $"Lambda body: {lambda}, is not of " +
                $"type {nameof(BlockSyntax)}");
        }

        GetHandlersFromLambda(
            body,
            config);
    }

    private static void GetHandlersFromLambda(
        BlockSyntax body,
        Config config)
    {
        var nodes = body
            .DescendantNodes()
            .OfType<ExpressionStatementSyntax>()
            .Where(x => $"{x}".Contains(HANDLERS_DEF));

        foreach (var n in nodes)
        {
            var statement = n
                .FlattenLine();

            var match = StringsPair
                .Match(statement);

            if (!match.Success)
            {
                continue;
            }

            if (config
                    .Clients
                    .Any(x => x.Name == match.Groups[1].Value) &&
                !config
                    .Handlers
                    .ContainsKey(match.Groups[1].Value))
            {
                config
                    .Handlers.Add(
                        match.Groups[1].Value,
                        match.Groups[2].Value);
            }
        }
    }
}
