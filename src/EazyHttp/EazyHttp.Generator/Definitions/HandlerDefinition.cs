namespace EazyHttp.Generator.Definitions;

internal sealed class HandlerDefinition(
  string typeExpression,
  bool isPrimary)
{
  public string TypeExpression { get; } = typeExpression;
  public bool IsPrimary { get; } = isPrimary;
}
