namespace EazyHttp.Generator.Contracts;

internal class Config
{
    public List<dynamic> Clients { get; } = new();

    public Dictionary<string, string> Handlers { get; } = new();

    private string _nameSpacePrefix = string.Empty;

    public string NameSpacePrefix {
        get {
            if (string.IsNullOrWhiteSpace(_nameSpacePrefix))
            {
                return string.Empty;
            }

            while (_nameSpacePrefix.EndsWith("."))
            {
                _nameSpacePrefix = _nameSpacePrefix
                    .Remove(
                        _nameSpacePrefix
                        .Length - 1,
                        1);
            }

            return $"{_nameSpacePrefix}.";
        }
    }

    public void SetNameSpacePrefix(
        string nameSpacePrefix) => _nameSpacePrefix 
        = nameSpacePrefix;
}
