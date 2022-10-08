namespace EazyHttp.Contracts;

/// <summary>
/// Options used to configure EazyClient instances.
/// </summary>
public class EazyClientOptions
{
    /// <summary>
    /// <para>Gets or sets the namespace prefix for the generated code.</para> 
    /// <para>If not provided, every file generated will have a namespace starting with: 
    /// "<see cref="EazyHttp"/>", otherwise the value provided will be used.</para>
    /// </summary>
    public string NameSpacePrefix { get; set; } = string.Empty;

    /// <summary>
    /// <para>Gets or sets the list of EazyClients (<see cref="IEazyHttpClient"/>).</para>
    /// <para>If the list is empty, one default client, with no base address 
    /// will be created (SharedHttpClient).</para>
    /// Collection items: <see cref="HttpClientDefinition"/>
    /// <code>
    /// // Default serializer
    /// new JsonSerializerOptions(JsonSerializerDefaults.Web);
    /// 
    /// // Default retry configuration
    /// new RetryConfiguration();
    /// 
    /// // Default encoding
    /// Encoding.UTF8;
    /// </code>
    /// </summary>
    public HttpClientsCollection EazyHttpClients { get; set; } = new();

    /// <summary>
    /// <para>Gets or sets a collection of http request headers matching an existing EazyClient.</para> 
    /// <para>If the provided key, matches an <see cref="HttpClientDefinition.Name"/> all headers 
    /// will be attached to any request made by the matching EazyClient.</para>
    /// </summary>
    public Dictionary<string, IEnumerable<RequestHeader>> PersistentHeaders { get; } = new();

    /// <summary>
    /// <para>Gets or sets the serializer options matching an existing EazyClient.</para> 
    /// <para>If the provided key, matches an <see cref="HttpClientDefinition.Name"/> the 
    /// serializer options will be used by the matching EazyClient.</para>
    /// </summary>
    public Dictionary<string, JsonSerializerOptions> SerializersOptions { get; } = new();

    /// <summary>
    /// <para>Gets or sets the retry policy matching an existing EazyClient.</para> 
    /// <para>If the provided key, matches an <see cref="HttpClientDefinition.Name"/> the 
    /// policy will be used by the matching EazyClient.</para>
    /// </summary>
    public Dictionary<string, RetryConfiguration> Retries { get; } = new();

    /// <summary>
    /// <para>Gets or sets the text encoding matching an existing EazyClient.</para> 
    /// <para>If the provided key, matches an <see cref="HttpClientDefinition.Name"/> the 
    /// encoding will be used by the matching EazyClient.</para>
    /// </summary>
    public Dictionary<string, Encoding> Encodings { get; } = new();

    /// <summary>
    /// <para>Gets or sets the name of a custom <see cref="HttpClientHandler"/> implementation
    /// matching an existing EazyClient.</para> 
    /// <para>If the provided key, matches an <see cref="HttpClientDefinition.Name"/> the 
    /// handler will be used by the matching EazyClient.</para>
    /// <example>
    /// !!The full name provided for the handler must match an existing implemetation, i.e.
    /// <code>
    /// HttpClientHandlers.Add("SharedHttpClient", "TestHttpClientHandler");
    ///
    /// public class TestHttpClientHandler : HttpClientHandler
    /// {
    ///     public TestHttpClientHandler()
    ///     {
    ///        AutomaticDecompression = DecompressionMethods.All;
    ///     }
    /// }
    /// </code>
    /// </example>
    /// </summary>
    public Dictionary<string, string> HttpClientHandlers { get; } = new();
}
