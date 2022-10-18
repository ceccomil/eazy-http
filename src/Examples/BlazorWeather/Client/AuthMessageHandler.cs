using Microsoft.AspNetCore.Components.WebAssembly.Authentication;
using Microsoft.AspNetCore.Components;

namespace BlazorWeather.Client;

public class AuthMessageHandler : BaseAddressAuthorizationMessageHandler
{
    public class AuthClientHandler : HttpClientHandler
    {
        public AuthClientHandler() : base()
        { }
    }

    public AuthMessageHandler(
        AuthClientHandler authClientHandler,
        IAccessTokenProvider accessTokenProvider,
        NavigationManager navigationManager
        )
        : base(
            accessTokenProvider,
            navigationManager)
    {
        InnerHandler = authClientHandler;
    }
}
