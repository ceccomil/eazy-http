namespace EazyHttp.Generator.Tests.Helpers;

internal sealed class EnrichedDiagnostics(
  Compilation compilation)
{
  public Compilation CurrentCompilation { get; } = compilation;
  public ImmutableArray<Diagnostic> Diagnostics { get; } = compilation.GetDiagnostics();

  public ImmutableArray<Diagnostic> Errors => Get(Diagnostics, DiagnosticSeverity.Error);

  public ImmutableArray<Diagnostic> Warnings => Get(Diagnostics, DiagnosticSeverity.Warning);

  public ImmutableArray<Diagnostic> Info => Get(Diagnostics, DiagnosticSeverity.Info);

  public ImmutableArray<Diagnostic> Hidden => Get(Diagnostics, DiagnosticSeverity.Hidden);

  public ImmutableArray<string> ErrorArguments => GetArguments(Errors);

  public ImmutableArray<string> WarningArguments => GetArguments(Warnings);

  public ImmutableArray<string> InfoArguments => GetArguments(Info);

  public ImmutableArray<string> HiddenArguments => GetArguments(Hidden);

  public string ReadableErrors => GetReadable(Errors);

  public string ReadableWarnings => GetReadable(Warnings);

  public string ReadableInfo => GetReadable(Info);

  public string ReadableHidden => GetReadable(Hidden);

  private static ImmutableArray<Diagnostic> Get(
    ImmutableArray<Diagnostic> diagnostics,
    DiagnosticSeverity severity)
  {
    var filtered = diagnostics
      .Where(x => x.Severity == severity)
      .ToImmutableArray();

    return filtered;
  }

  private static string GetReadable(ImmutableArray<Diagnostic> diagnostics)
  {
    var texts = diagnostics
      .Select(x => x.ToString());

    return string.Join(Environment.NewLine, texts);
  }

  private static ImmutableArray<string> GetArguments(ImmutableArray<Diagnostic> diagnostics)
  {
    List<string> args = [];

    foreach (var d in diagnostics)
    {
      try
      {
        var pi = d
          .GetType()
          .GetProperty("Arguments", BindingFlags.NonPublic | BindingFlags.Instance)!;

        var objArgs = pi
          .GetValue(d) as IReadOnlyList<object>;

        args.AddRange(objArgs!.Select(x => x.ToString()!));
      }
      catch
      {
        // do nothing!
      }
    }

    return [.. args.Distinct()];
  }

  public override string ToString()
  {
    var sb = new StringBuilder();

    if (Errors.Length > 0)
    {
      sb.AppendLine("Errors:");
      sb.AppendLine(ReadableErrors);
      sb.AppendLine("########################################");
      sb.AppendLine();
    }

    if (Warnings.Length > 0)
    {
      sb.AppendLine("Warnings:");
      sb.AppendLine(ReadableWarnings);
      sb.AppendLine("########################################");
      sb.AppendLine();
    }

    if (Info.Length > 0)
    {
      sb.AppendLine("Info:");
      sb.AppendLine(ReadableInfo);
      sb.AppendLine("########################################");
      sb.AppendLine();
    }

    if (Hidden.Length > 0)
    {
      sb.AppendLine("Hidden:");
      sb.AppendLine(ReadableHidden);
      sb.AppendLine("########################################");
      sb.AppendLine();
    }

    return sb.ToString();
  }
}
