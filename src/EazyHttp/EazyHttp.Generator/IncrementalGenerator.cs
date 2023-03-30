namespace EazyHttp.Generator;

[Generator]
public class IncrementalGenerator : IIncrementalGenerator
{
    public void Initialize(
        IncrementalGeneratorInitializationContext context)
    {
        var configs = context
            .SyntaxProvider
            .CreateSyntaxProvider(
                predicate: static (x, _) => Nodes.IsConfigFound(x),
                transform: (x, _) => Nodes.GetConfig(x))
            .Where(x => x is not null && x.Clients.Any())
            .Collect();

        var compilationAndConfigs = context
            .CompilationProvider
            .Combine(configs);

        context
            .RegisterSourceOutput(
                compilationAndConfigs,
                static (x, y) => Execute(
                    x,
                    y.Right));
    }

    private static void Execute(
        SourceProductionContext context,
        ImmutableArray<Config> configs)
    {
        if (configs.IsDefaultOrEmpty)
        {
            return;
        }

        var subset = configs
            .Distinct()
            .ToList();

        CodeGenerator
            .Execute(
                context,
                subset
                .Last());
    }
}
