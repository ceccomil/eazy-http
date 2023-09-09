namespace EazyHttp.Generator.Helpers;

internal static class StatementsExtensions
{
    public static Regex StringsPair { get; } = new("\"(.+)\",\"(.+)\"");
    public static Regex ValuesPair { get; } = new("\\(([^\\(]+),([^\\)]+)\\)");

    public static string VarQualifier = $"$@VAR-{Guid.NewGuid()}$";

    public static string BackToVariable(
        this string address) => address
            .Replace("\"", "")
            .Replace(VarQualifier, "");

    public static string ToVariableStatement(
        this ExpressionStatementSyntax statement)
    {
        var value = statement
            .FlattenLine();

        var match = StringsPair
            .Match(value);

        if (match.Success)
        {
            return value;
        }

        match = ValuesPair
            .Match(value);

        if (!match.Success)
        {
            return value;
        }

        return value
            .AdjustStatement(match.Groups[1].Value)
            .AdjustStatement(match.Groups[2].Value);
    }

    private static string AdjustStatement(
        this string statement,
        string parameter)
    {
        if (parameter.Length > 2 &&
            parameter.First() == '"' &&
            parameter.Last() == '"')
        {
            return statement;
        }

        var newParam = $"\"{VarQualifier}{parameter}\"";

        return statement
            .Replace(
                parameter,
                newParam);
    }

    public static string FlattenLine(
        this string statement) => statement
            .Replace("\r", "")
            .Replace("\n", "")
            .Replace("HttpClientDefinition", "")
            .Replace(" ", "");

    public static string FlattenLine(
        this ExpressionStatementSyntax statement) => $"{statement}"
            .FlattenLine();
}
