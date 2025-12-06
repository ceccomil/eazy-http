namespace EazyHttp.Generator.Helpers;

internal static class InitializationContextExtensions
{
  public static void DefaultSourceOutputRegistration(
    this IncrementalGeneratorInitializationContext context,
    Action<SourceProductionContext, ImmutableArray<EazyClientOptionsDefinition>, bool> execution)
  {
    var definitionsProvider = context
      .SyntaxProvider
      .CreateSyntaxProvider(
        predicate: static (x, _) => x.IsConfigureEazyHttpClients(),
        transform: static (x, _) => x.GetClientsOptions())
      .Where(x => x is not null)
      .Collect();

    var nullableProvider = context
      .CompilationProvider
      .Select(static (x, _) =>
          x.Options.NullableContextOptions is not NullableContextOptions.Disable);

    var combinedProvider = definitionsProvider
        .Combine(nullableProvider);

    context.RegisterSourceOutput(
      combinedProvider,
      (x, y) => execution(x, y.Left!, y.Right));
  }
}
