namespace CrudApi.Authentication;

[AttributeUsage(validOn: AttributeTargets.Class | AttributeTargets.Method)]
public class AuthenticateAttribute : Attribute, IAsyncActionFilter
{
    private const string API_KEY = "X-SharedSecret";

    public async Task OnActionExecutionAsync(
        ActionExecutingContext context,
        ActionExecutionDelegate next)
    {
        context
            .Result = new ContentResult()
        {
            StatusCode = 401,
            Content = "Not authenticated!"
        };

        var config = context
            .HttpContext
            .RequestServices
            .GetRequiredService<IConfiguration>();

        var isAuth = context
            .HttpContext
            .Request
            .Headers
            .TryGetValue(
                API_KEY,
                out var secret);

        var logger = context
            .HttpContext
            .RequestServices
            .GetRequiredService<ICaptainLogger<AuthenticateAttribute>>();

        if (!isAuth)
        {
            logger
                .WarningLog("Authentication header not provided!");

            return;
        }

        var expectedSecret = config["ApiKey"];

        if (expectedSecret is null ||
            expectedSecret.Length < 16)
        {
            throw new ApplicationException(
                "Application secret is not defined or too weak!");
        }
       
        if (expectedSecret != secret)
        {
            logger
                .WarningLog("Authentication header value is wrong!");

            return;
        }

        logger
            .InformationLog("Request is authenticated!");

        context.Result = null;
        await next();
    }
}
