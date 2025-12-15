namespace EazyHttp.Configuration;

/// <summary>
/// Options used to configure EazyHttp clients and to drive the source generator.
/// </summary>
/// <remarks>
/// <para>
/// This type is intended to live in the abstractions assembly and be shared between:
/// </para>
/// <list type="bullet">
///   <item>
///     <description>
///       Consumer code (which configures <see cref="EazyClientOptions"/> at startup).
///     </description>
///   </item>
///   <item>
///     <description>
///       The EazyHttp source generator (which inspects <see cref="NamespacePrefix"/> and
///       <see cref="Clients"/> to generate named, strongly typed HTTP clients).
///     </description>
///   </item>
///   <item>
///     <description>
///       The EazyHttp runtime library (which uses the resolvers and dictionaries to build
///       actual <see cref="HttpClient"/> instances).
///     </description>
///   </item>
/// </list>
/// </remarks>
public sealed class EazyClientOptions
{
  /// <summary>
  /// Gets or sets the namespace prefix for generated client types.
  /// </summary>
  /// <remarks>
  /// <para>
  /// The source generator uses this value as the root namespace for all generated
  /// interfaces and implementations. If not explicitly set, the default value is
  /// the name of the <see cref="EazyHttp"/> root namespace.
  /// </para>
  /// <para>
  /// This must be a literal string.
  /// </para>
  /// </remarks>
  public string NamespacePrefix { get; set; } = nameof(EazyHttp);

  /// <summary>
  /// Gets the list of HTTP client definitions used by EazyHttp.
  /// </summary>
  /// <remarks>
  /// <para>
  /// Each <see cref="HttpClientDefinition"/> describes a logical HTTP client
  /// (name, base address, generation flags, etc.).
  /// </para>
  /// <para>
  /// The source generator inspects this collection (together with
  /// <see cref="NamespacePrefix"/>) to decide which named clients to generate.
  /// </para>
  /// </remarks>
  public List<HttpClientDefinition> Clients { get; } = [];

  /// <summary>
  /// Gets or sets an optional resolver used to obtain serializer options
  /// for a specific client at runtime.
  /// </summary>
  /// <remarks>
  /// <para>
  /// The runtime library can invoke this delegate to compute an
  /// <see cref="JsonSerializerOptions"/> instance for a given client,
  /// typically using the information in the provided <see cref="ClientContext"/>
  /// (for example, the client name and the <see cref="System.IServiceProvider"/>).
  /// </para>
  /// <para>
  /// If this delegate is <c>null</c>, EazyHttp will fall back to looking up
  /// the client in <see cref="RequestSerializerOptions"/> and, if no match is found,
  /// to a runtime-specific default.
  /// </para>
  /// </remarks>
  public Func<ClientContext, JsonSerializerOptions?>? ResolveRequestSerializer { get; set; }

  /// <summary>
  /// Gets or sets an optional resolver used to obtain serializer options
  /// for a specific client at runtime.
  /// </summary>
  /// <remarks>
  /// <para>
  /// The runtime library can invoke this delegate to compute an
  /// <see cref="JsonSerializerOptions"/> instance for a given client,
  /// typically using the information in the provided <see cref="ClientContext"/>
  /// (for example, the client name and the <see cref="System.IServiceProvider"/>).
  /// </para>
  /// <para>
  /// If this delegate is <c>null</c>, EazyHttp will fall back to looking up
  /// the client in <see cref="ResponseSerializerOptions"/> and, if no match is found,
  /// to a runtime-specific default.
  /// </para>
  /// </remarks>
  public Func<ClientContext, JsonSerializerOptions?>? ResolveResponseSerializer { get; set; }

  /// <summary>
  /// Gets or sets an optional resolver used to obtain retry configuration
  /// for a specific client at runtime.
  /// </summary>
  /// <remarks>
  /// <para>
  /// If this delegate is provided, it is invoked to obtain a
  /// <see cref="RetryConfiguration"/> for the client identified by the
  /// given <see cref="ClientContext"/>.
  /// </para>
  /// <para>
  /// If this delegate is <c>null</c>, EazyHttp will fall back to
  /// <see cref="Retries"/> and, if no entry is found for the client,
  /// to a runtime-specific default retry configuration.
  /// </para>
  /// </remarks>
  public Func<ClientContext, RetryConfiguration?>? ResolveRetry { get; set; }

  /// <summary>
  /// Gets or sets an optional resolver used to obtain the text encoding
  /// for a specific client at runtime.
  /// </summary>
  /// <remarks>
  /// <para>
  /// If this delegate is provided, it is invoked to obtain an
  /// <see cref="Encoding"/> for the client identified by the given
  /// <see cref="ClientContext"/>.
  /// </para>
  /// <para>
  /// If this delegate is <c>null</c>, EazyHttp will fall back to
  /// <see cref="Encodings"/> and, if no entry is found for the client,
  /// to a runtime-specific default (commonly <see cref="Encoding.UTF8"/>).
  /// </para>
  /// </remarks>
  public Func<ClientContext, Encoding?>? ResolveEncoding { get; set; }

  /// <summary>
  /// Gets or sets an optional resolver used to obtain persistent headers
  /// for a specific client at runtime.
  /// </summary>
  /// <remarks>
  /// <para>
  /// If this delegate is provided, it is invoked to obtain a sequence of
  /// <see cref="RequestHeader"/> instances that will be attached to all
  /// requests issued by the client identified by the given
  /// <see cref="ClientContext"/>.
  /// </para>
  /// <para>
  /// If this delegate is <c>null</c>, EazyHttp will fall back to
  /// <see cref="PersistentHeaders"/> and, if no entry is found for the client,
  /// will treat the client as having no additional persistent headers.
  /// </para>
  /// </remarks>
  public Func<ClientContext, IEnumerable<RequestHeader>?>? ResolveHeaders { get; set; }

  /// <summary>
  /// Gets a mapping from client name to serializer options.
  /// </summary>
  /// <remarks>
  /// <para>
  /// This dictionary provides a simple, configuration-based way to map
  /// a logical client name to an <see cref="JsonSerializerOptions"/> instance.
  /// </para>
  /// <para>
  /// At runtime, this dictionary may be used directly, or via the
  /// <see cref="ResolveRequestSerializer"/> delegate if that delegate chooses
  /// to consult it.
  /// </para>
  /// </remarks>
  public Dictionary<string, JsonSerializerOptions> RequestSerializerOptions { get; } = [];

  /// <summary>
  /// Gets a mapping from client name to serializer options.
  /// </summary>
  /// <remarks>
  /// <para>
  /// This dictionary provides a simple, configuration-based way to map
  /// a logical client name to an <see cref="JsonSerializerOptions"/> instance.
  /// </para>
  /// <para>
  /// At runtime, this dictionary may be used directly, or via the
  /// <see cref="ResolveResponseSerializer"/> delegate if that delegate chooses
  /// to consult it.
  /// </para>
  /// </remarks>
  public Dictionary<string, JsonSerializerOptions> ResponseSerializerOptions { get; } = [];

  /// <summary>
  /// Gets a mapping from client name to retry configuration.
  /// </summary>
  /// <remarks>
  /// <para>
  /// This dictionary provides a simple, configuration-based way to map
  /// a logical client name to a <see cref="RetryConfiguration"/>.
  /// </para>
  /// <para>
  /// At runtime, this dictionary may be used directly, or via the
  /// <see cref="ResolveRetry"/> delegate if that delegate chooses
  /// to consult it.
  /// </para>
  /// </remarks>
  public Dictionary<string, RetryConfiguration> Retries { get; } = [];

  /// <summary>
  /// Gets a mapping from client name to text encoding.
  /// </summary>
  /// <remarks>
  /// <para>
  /// This dictionary provides a simple, configuration-based way to map
  /// a logical client name to an <see cref="Encoding"/>.
  /// </para>
  /// <para>
  /// At runtime, this dictionary may be used directly, or via the
  /// <see cref="ResolveEncoding"/> delegate if that delegate chooses
  /// to consult it.
  /// </para>
  /// </remarks>
  public Dictionary<string, Encoding> Encodings { get; } = [];

  /// <summary>
  /// Gets a mapping from client name to persistent headers.
  /// </summary>
  /// <remarks>
  /// <para>
  /// This dictionary provides a simple, configuration-based way to map
  /// a logical client name to a sequence of <see cref="RequestHeader"/> values
  /// that should be attached to all outgoing requests for that client.
  /// </para>
  /// <para>
  /// At runtime, this dictionary may be used directly, or via the
  /// <see cref="ResolveHeaders"/> delegate if that delegate chooses
  /// to consult it.
  /// </para>
  /// </remarks>
  public Dictionary<string, IEnumerable<RequestHeader>> PersistentHeaders { get; } = [];

  /// <summary>
  /// Gets a mapping from logical EazyHttp client names to the <see cref="HttpMessageHandler"/> type
  /// that should be used in the generated <see cref="HttpClient"/> pipeline.
  /// </summary>
  /// <remarks>
  /// <para>
  /// The key is the logical client name as configured in <see cref="Clients"/> (for example
  /// <c>"Test1"</c>).
  /// The value is a <see cref="Type"/> representing a concrete
  /// <see cref="HttpMessageHandler"/> implementation that should be used for that client.
  /// </para>
  /// <para>
  /// At code-generation time the EazyHttp source generator inspects the configured type:
  /// if it derives from <see cref="DelegatingHandler"/>, it is registered as a delegating handler
  /// using <c>AddHttpMessageHandler&lt;THandler&gt;()</c>; otherwise it is used as the primary
  /// handler via <c>ConfigurePrimaryHttpMessageHandler&lt;THandler&gt;()</c>.
  /// </para>
  /// <para>
  /// Only a single handler type can be configured per client via this property. If you need a more
  /// complex pipeline (for example multiple delegating handlers), encapsulate that pipeline in your
  /// own <see cref="DelegatingHandler"/> chain and register the outermost handler type here.
  /// </para>
  /// <para>
  /// The type must be accessible to the project that consumes EazyHttp. The generator will emit
  /// registrations using the resolved type; if the handler type cannot be found or does not derive
  /// from <see cref="HttpMessageHandler"/>, the generated code will fail to compile with a normal
  /// C# compiler error.
  /// </para>
  /// </remarks>
  public Dictionary<string, Type> HttpClientHandlers { get; } = [];
}
