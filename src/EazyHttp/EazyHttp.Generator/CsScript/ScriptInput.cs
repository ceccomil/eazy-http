using System.Reflection;

namespace EazyHttp.Generator.CsScript;

internal class ScriptInput
{
    public string Label { get; set; } = null!;

    public string Code { get; set; } = null!;

    public CsGlobals? Globals { get; set; }

    internal ICollection<Assembly> LoadedAssemblies { get; set; } = new List<Assembly>();

    public IEnumerable<string>? Refs { get; set; }

    public IEnumerable<string>? Imports { get; set; }

    public override string ToString() => $"[{Label}, {Code}]";
}
