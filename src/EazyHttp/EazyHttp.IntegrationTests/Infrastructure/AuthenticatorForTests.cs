namespace EazyHttp.IntegrationTests.Infrastructure;

public interface IAuthenticatorForTests
{
  public const string KEY = "123e4567-e89b-12d3-a456-426614174000";

  public Guid ApiKey { get; }
}

internal sealed class AuthenticatorForTests
  : IAuthenticatorForTests
{
  public Guid ApiKey { get; } = Guid.Parse(IAuthenticatorForTests.KEY);
}

internal class AuthMessageHandler(
  IAuthenticatorForTests auth) : BaseAuthHanler(auth.ApiKey);

internal class BaseAuthHanler(Guid key) : DelegatingHandler
{
  private readonly Guid _key = key;

  protected override Task<HttpResponseMessage> SendAsync(
    HttpRequestMessage request, 
    CancellationToken cancellationToken)
  {
    request.Headers.Add(
      "X-API-KEY", 
      $"{_key}");
    
    return base.SendAsync(request, cancellationToken);
  }
}
