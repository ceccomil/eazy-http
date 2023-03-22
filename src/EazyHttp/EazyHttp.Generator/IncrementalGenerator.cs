namespace EazyHttp.Generator;

public class IncrementalGenerator : IIncrementalGenerator
{
    public void Initialize(
        IncrementalGeneratorInitializationContext context)
    {
        var config = context
            .SyntaxProvider
            .CreateSyntaxProvider(
                predicate: static (x, _) => Nodes.IsConfigFound(x),
                transform: (x, _) => Nodes.GetConfig(x))
            .Where(x => x is not null)
            .Collect();


        context
            .RegisterSourceOutput(
                config,
                Execute);
    }

    private static void Execute(
        SourceProductionContext context,
        ImmutableArray<Config> configs)
    {
        var config = configs
            .Single();

        CodeGenerator
            .Execute(
                context,
                config);
    }
}
