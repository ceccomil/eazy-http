﻿namespace EazyHttp.Contracts;

/// <summary>
/// TODO Documentation
/// </summary>
public class EazyClientOptions
{
    /// <summary>
    /// TODO Documentation
    /// </summary>
    public HttpClientsCollection EazyHttpClients { get; set; } = new();

    /// <summary>
    /// TODO documentation
    /// </summary>
    public Dictionary<string, IEnumerable<RequestHeader>> PersistentHeaders { get; } = new();

    /// <summary>
    /// TODO documentation
    /// </summary>
    public Dictionary<string, JsonSerializerOptions> SerializersOptions { get; } = new();
}