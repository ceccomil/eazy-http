using System.Text.Json;

namespace EazyHttp.Generator.Receivers;

internal static class HttpClientReceiver
{
  public static bool IsConfigureEazyHttpClients(this SyntaxNode node)
  {
    if (node is not InvocationExpressionSyntax invocation)
    {
      return false;
    }

    var name = "";

    // services.ConfigureEazyHttpClients(...)
    if (invocation.Expression is MemberAccessExpressionSyntax memberAccess)
    {
      name = memberAccess.Name.Identifier.ValueText;
    }

    // ServiceCollectionExtensions.ConfigureEazyHttpClients(...)
    if (invocation.Expression is IdentifierNameSyntax identifier)
    {
      name = identifier.Identifier.ValueText;
    }

    return string.Equals(
      name,
      ConfigureMethodName,
      StringComparison.Ordinal);
  }

  public static EazyClientOptionsDefinition? GetClientsOptions(this GeneratorSyntaxContext context)
  {
    if (context.Node is not InvocationExpressionSyntax invocation)
    {
      return null;
    }

    var semanticModel = context.SemanticModel;

    var methodSymbol = GetConfigureMethodSymbol(
      invocation,
      semanticModel);

    if (methodSymbol is null)
    {
      return null;
    }

    var options = new EazyClientOptionsDefinition();

    PopulateClientsFromConfigInvocation(
      invocation,
      semanticModel,
      options);

    if (options.Clients.Count == 0)
    {
      // No clients defined
      return null;
    }

    return options;
  }

  private static IMethodSymbol? GetConfigureMethodSymbol(
    InvocationExpressionSyntax invocation,
    SemanticModel semanticModel)
  {
    // Make sure this is OUR ConfigureEazyHttpClients
    var symbolInfo = semanticModel.GetSymbolInfo(invocation);

    var methodSymbol = symbolInfo.Symbol as IMethodSymbol
      ?? symbolInfo.CandidateSymbols.OfType<IMethodSymbol>()
      .FirstOrDefault();

    if (methodSymbol is null ||
      !string.Equals(
        methodSymbol.Name,
        ConfigureMethodName,
        StringComparison.Ordinal))
    {
      return null;
    }

    // We expect: this IServiceCollection services, Action<EazyClientOptions>? config = null
    var parameters = methodSymbol.Parameters;
    if (parameters.Length != 1)
    {
      return null;
    }

    var configParam = parameters[0];

    var compilation = semanticModel.Compilation;

    var action1Symbol = compilation.GetTypeByMetadataName("System.Action`1");

    var eazyClientOptionsSymbol = compilation.GetTypeByMetadataName(
      typeof(EazyClientOptions).FullName!);

    if (action1Symbol is null ||
        eazyClientOptionsSymbol is null)
    {
      // Types not found in this compilation => not our method / no point proceeding.
      return null;
    }

    // Second parameter must be Action<EazyClientOptions> (nullable is fine)
    if (configParam.Type is not INamedTypeSymbol configType)
    {
      return null;
    }

    // Ignore nullability and compare generic definition: Action<T>
    if (!SymbolEqualityComparer.Default.Equals(configType.ConstructedFrom, action1Symbol))
    {
      return null;
    }

    if (configType.TypeArguments.Length != 1)
    {
      return null;
    }

    var typeArg = configType.TypeArguments[0];

    if (!SymbolEqualityComparer.Default.Equals(typeArg, eazyClientOptionsSymbol))
    {
      return null;
    }

    return methodSymbol;
  }

  private static void PopulateClientsFromConfigInvocation(
    InvocationExpressionSyntax invocation,
    SemanticModel semanticModel,
    EazyClientOptionsDefinition options)
  {
    // Expect exactly one argument: the config lambda
    if (invocation.ArgumentList.Arguments.Count != 1)
    {
      return;
    }

    var arg = invocation.ArgumentList.Arguments[0];

    if (arg.Expression is not LambdaExpressionSyntax lambda)
    {
      return;
    }

    var parameterName = GetLambdaParameterName(lambda);

    if (string.IsNullOrWhiteSpace(parameterName))
    {
      return;
    }

    var statements = GetLambdaStatements(lambda);

    foreach (var statement in statements)
    {
      if (statement is not ExpressionStatementSyntax expressionStatement)
      {
        continue;
      }

      switch (expressionStatement.Expression)
      {
        case InvocationExpressionSyntax innerInvocation:
          {
            TryHandleInvocation(
              innerInvocation,
              semanticModel,
              parameterName!,
              options);

            break;
          }

        case AssignmentExpressionSyntax assignment:
          {
            TryHandleAssignment(
              assignment,
              parameterName!,
              options);

            break;
          }
      }
    }
  }

  private static void TryHandleInvocation(
    InvocationExpressionSyntax invocation,
    SemanticModel semanticModel,
    string optionsParameterName,
    EazyClientOptionsDefinition options)
  {
    // x.Something.Add(...)
    if (invocation.Expression is not MemberAccessExpressionSyntax addAccess)
    {
      return;
    }

    if (!string.Equals(
      addAccess.Name.Identifier.ValueText, 
      nameof(ICollection<>.Add), 
      StringComparison.Ordinal))
    {
      return;
    }

    if (addAccess.Expression is not MemberAccessExpressionSyntax collectionAccess)
    {
      return;
    }

    if (collectionAccess.Expression is not IdentifierNameSyntax identifier ||
      !string.Equals(
        identifier.Identifier.ValueText, 
        optionsParameterName, 
        StringComparison.Ordinal))
    {
      return;
    }

    var collectionName = collectionAccess.Name.Identifier.ValueText;

    switch (collectionName)
    {
      case nameof(EazyClientOptions.Clients):
        TryHandleClientsAddInvocation(
          invocation, 
          semanticModel, 
          optionsParameterName, 
          options);
        break;

      case nameof(EazyClientOptions.SerializerOptions):
        TryHandleDictionaryAddInvocation(
          invocation, 
          options.SerializerOptions);
        break;

      case nameof(EazyClientOptions.Retries):
        TryHandleDictionaryAddInvocation(
          invocation,
          options.Retries);
        break;

      case nameof(EazyClientOptions.Encodings):
        TryHandleDictionaryAddInvocation(
          invocation,
          options.Encodings);
        break;

      case nameof(EazyClientOptions.PersistentHeaders):
        TryHandleDictionaryAddInvocation(
          invocation,
          options.PersistentHeaders);
        break;

      case nameof(EazyClientOptions.HttpClientHandlerTypeNames):
        TryHandleDictionaryAddInvocation(
          invocation,
          options.HttpClientHandlers);
        break;
    }
  }

  private static string? GetLambdaParameterName(
    LambdaExpressionSyntax lambda)
  {
    switch (lambda)
    {
      case SimpleLambdaExpressionSyntax simple:
        {
          return simple.Parameter.Identifier.ValueText;
        }

      case ParenthesizedLambdaExpressionSyntax par
        when par.ParameterList.Parameters.Count == 1:
        {
          return par.ParameterList.Parameters[0].Identifier.ValueText;
        }

      default:
        {
          return null;
        }
    }
  }

  private static SyntaxList<StatementSyntax> GetLambdaStatements(
    LambdaExpressionSyntax lambda)
  {
    if (lambda.Body is BlockSyntax block)
    {
      return block.Statements;
    }

    if (lambda.Body is ExpressionSyntax expr)
    {
      // Expression-bodied lambda: options => options.Clients.Add(...)
      return SyntaxFactory.List<StatementSyntax>(
        new[]
        {
          SyntaxFactory.ExpressionStatement(expr)
        });
    }

    return default;
  }

  private static void TryHandleClientsAddInvocation(
    InvocationExpressionSyntax invocation,
    SemanticModel semanticModel,
    string optionsParameterName,
    EazyClientOptionsDefinition options)
  {
    // We are looking for: x.Clients.Add(...)
    if (invocation.Expression is not MemberAccessExpressionSyntax addAccess)
    {
      return;
    }

    if (!string.Equals(
      addAccess.Name.Identifier.ValueText,
      nameof(EazyClientOptions.Clients.Add),
      StringComparison.Ordinal))
    {
      return;
    }

    if (addAccess.Expression is not MemberAccessExpressionSyntax clientsAccess)
    {
      return;
    }

    if (!string.Equals(
      clientsAccess.Name.Identifier.ValueText,
      nameof(EazyClientOptions.Clients),
      StringComparison.Ordinal))
    {
      return;
    }

    if (clientsAccess.Expression is not IdentifierNameSyntax identifier)
    {
      return;
    }

    if (!string.Equals(
      identifier.Identifier.ValueText,
      optionsParameterName,
      StringComparison.Ordinal))
    {
      return;
    }

    // At this point we know this is: optionsParam.Clients.Add(...)
    if (invocation.ArgumentList.Arguments.Count != 1)
    {
      return;
    }

    var clientArgExpr = invocation.ArgumentList.Arguments[0].Expression;

    if (clientArgExpr is not BaseObjectCreationExpressionSyntax creation)
    {
      return;
    }

    var args = creation.ArgumentList?.Arguments;

    if (args is null ||
      args.Value.Count == 0)
    {
      return;
    }

    var name = "";
    string? baseAddress = null;
    var optionalHeadersInMethods = false;

    // name (required)
    var nameConst = semanticModel
      .GetConstantValue(args.Value[0].Expression);

    if (nameConst.HasValue)
    {
      name = $"{nameConst.Value}";
    }

    if (string.IsNullOrWhiteSpace(name))
    {
      // only support constant string names
      return;
    }

    // baseAddress 
    var baExpr = args.Value[1]
    .Expression
    .NormalizeWhitespace();

    if (baExpr is not LiteralExpressionSyntax literal ||
      !literal.IsKind(SyntaxKind.NullLiteralExpression))
    {
      baseAddress = baExpr.ToFullString();
    }

    // optionalHeadersInMethods
    var thirdConst = semanticModel
      .GetConstantValue(args.Value[2].Expression);

    if (thirdConst.HasValue &&
      thirdConst.Value is bool boolValue)
    {
      optionalHeadersInMethods = boolValue;
    }

    options
      .Clients
      .Add(
        new HttpClientDefinition(
          name,
          baseAddress,
          optionalHeadersInMethods));
  }

  private static void TryHandleAssignment(
    AssignmentExpressionSyntax assignment,
    string optionsParameterName,
    EazyClientOptionsDefinition options)
  {
    // Expect: x.Property = ...
    if (assignment.Left is not MemberAccessExpressionSyntax memberAccess)
    {
      return;
    }

    // Must be: <optionsParam>.<Property>
    if (memberAccess.Expression is not IdentifierNameSyntax identifier ||
      !string.Equals(
        identifier.Identifier.ValueText,
        optionsParameterName,
        StringComparison.Ordinal))
    {
      return;
    }

    var propertyName = memberAccess
      .Name
      .Identifier
      .ValueText;

    switch (propertyName)
    {
      case nameof(EazyClientOptions.NamespacePrefix):
        HandleNamespacePrefixAssignment(
          assignment,
          options);
        break;

      case nameof(EazyClientOptions.ResolveRetry):
        HandleResolverAssignment(
          assignment,
          static (opts, expr) => opts.ResolveRetryExpression = expr,
          options);
        break;

      case nameof(EazyClientOptions.ResolveEncoding):
        HandleResolverAssignment(
          assignment,
          static (opts, expr) => opts.ResolveEncodingExpression = expr,
          options);
        break;

      case nameof(EazyClientOptions.ResolveHeaders):
        HandleResolverAssignment(
          assignment,
          static (opts, expr) => opts.ResolveHeadersExpression = expr,
          options);
        break;
    }
  }

  private static void TryHandleDictionaryAddInvocation(
    InvocationExpressionSyntax invocation,
    Dictionary<string, string> dictionary)
  {
    // Must have exactly 2 arguments: key, value
    var args = invocation.ArgumentList.Arguments;
    
    if (args.Count != 2)
    {
      return;
    }

    // Key: must be a string literal
    if (args[0].Expression is not LiteralExpressionSyntax keyLiteral ||
      !keyLiteral.IsKind(SyntaxKind.StringLiteralExpression))
    {
      return;
    }

    var key = keyLiteral.Token.ValueText;
    if (string.IsNullOrWhiteSpace(key))
    {
      return;
    }

    // Value: keep expression as code
    var valueExpr = args[1].Expression;
    var valueCode = valueExpr
      .NormalizeWhitespace()
      .ToFullString();

    dictionary[key] = valueCode;
  }

  private static void HandleNamespacePrefixAssignment(
    AssignmentExpressionSyntax assignment,
    EazyClientOptionsDefinition options)
  {
    if (assignment.Right is not LiteralExpressionSyntax literal ||
      !literal.IsKind(SyntaxKind.StringLiteralExpression))
    {
      // Not a plain string literal ignore, keep default
      return;
    }

    var value = literal
      .NormalizeWhitespace()
      .Token
      .ValueText;

    if (!string.IsNullOrWhiteSpace(value))
    {
      options.NamespacePrefix = value;
    }
  }

  private static void HandleResolverAssignment(
    AssignmentExpressionSyntax assignment,
    Action<EazyClientOptionsDefinition, string> setExpression,
    EazyClientOptionsDefinition options)
  {
    // We accept anything the compiler will later accept
    var valueExpr = assignment.Right;

    var valueCode = valueExpr
      .NormalizeWhitespace()
      .ToFullString();

    setExpression(options, valueCode);
  }
}
