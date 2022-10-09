namespace EazyHttp.Generator.CsScript;

internal class ExpressionEvaluator
{
    private InteractiveAssemblyLoader? _loader = null;
    private ScriptOptions? _scriptOpts = null;
    private Assembly? _mscorlib = null;
    private Assembly? _linq = null;

    internal Exception? EvalException { get; private set; }

    internal ScriptInput InitInput(
        ScriptInput input)
    {
        _mscorlib ??= typeof(object)
                .GetTypeInfo()
                .Assembly;

        _linq ??= typeof(Enumerable)
                .GetTypeInfo()
                .Assembly;

        input
            .LoadedAssemblies
            .Add(_mscorlib);

        input
            .LoadedAssemblies
            .Add(_linq);

        if (_scriptOpts is null)
        {
            _scriptOpts = ScriptOptions.Default;

            _scriptOpts = _scriptOpts.AddReferences(_mscorlib);
            _scriptOpts = _scriptOpts.AddReferences(_linq);

            // using
            _scriptOpts = _scriptOpts
                .AddImports("System")
                .AddImports("System.Linq")
                .AddImports("System.Collections.Generic")
                .AddImports("System.Threading.Tasks");
        }

        if (_loader is null)
        {
            _loader = new();
            _loader
                .RegisterDependency(_mscorlib);

            _loader
                .RegisterDependency(_linq);
        }

        AddRefsAndImports(
            input.Refs,
            input.Imports,
            input.LoadedAssemblies);

        if (input.Globals is not CsGlobals gO)
        {
            return input;
        }

        Type? type = null;
        foreach (var a in input.LoadedAssemblies)
        {
            type = a.GetType(gO.TypeName);
        }

        if (type is null)
        {
            return input;
        }

        input
            .Globals
            .Type = type;

        return input;
    }

    internal async Task<TResult> GetEvaluated<TResult>(
        ScriptInput input)
        where TResult : class
    {
        try
        {
            var instance = input
                .Globals?
                .Args ?? new object();

            var script = CSharpScript
                .Create(
                string.Empty,
                options: _scriptOpts,
                globalsType: instance.GetType(),
                assemblyLoader: _loader);

            var state = await script
                .RunAsync(instance);

            state = await state
                .ContinueWithAsync(input.Code);

            return (state.ReturnValue as TResult)!;

        }
        catch (Exception ex)
        {
            EvalException = ex;
        }
        return default!;
    }

    private void AddRefsAndImports(
        IEnumerable<string>? refs,
        IEnumerable<string>? imports,
        ICollection<Assembly> assemblies)
    {
        if (_loader is null || _scriptOpts is null)
        {
            return;
        }

        if (refs is null && imports is null)
        {
            return;
        }

        if (refs is not null)
        {
            foreach (var r in refs)
            {
                var a = r.StartsWith("@")
                    ? Assembly
                        .LoadFrom(
                            r.Substring(1))
                    : Assembly
                        .Load(r);

                _scriptOpts = _scriptOpts
                    .AddReferences(a);

                _loader
                    .RegisterDependency(a);

                if (!assemblies.Contains(a))
                {
                    assemblies.Add(a);
                }
            }
        }

        if (imports is not null)
        {
            foreach (var i in imports)
            {
                _scriptOpts = _scriptOpts.AddImports(i);
            }
        }
    }

}
