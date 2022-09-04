namespace EazyHttp.Contracts;

/// <summary>
/// TODO Documentation
/// </summary>
public class EazyClientOptions
{
    /// <summary>
    /// TODO Documentation
    /// </summary>
    public JsonSerializerOptions SerializerOptions { get; set; } = new(
            JsonSerializerDefaults.Web);

    /// <summary>
    /// TODO Documentation
    /// </summary>
    public HttpClientsCollection EazyHttpClients { get; set; } = new();

    /// <summary>
    /// TODO documentation
    /// </summary>
    public Dictionary<string, IEnumerable<RequestHeader>> PersistentHeaders { get; } = new();
}
