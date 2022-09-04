namespace EazyHttp.Generator.CsScript;

internal class CsGlobals
{
    public object Args { get; set; } = null!;

    public string TypeName { get; set; } = null!;

    internal Type? Type { get; set; }
}
