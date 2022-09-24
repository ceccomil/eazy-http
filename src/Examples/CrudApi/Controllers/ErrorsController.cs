namespace CrudApi.Controllers;

[ApiController]
[Route("[controller]")]
[Produces("text/plain")]
public class ErrorsController
{
    private readonly ICaptainLogger _logger;

    public ErrorsController(
        ICaptainLogger<OrdersController> logger)
    {
        _logger = logger;
    }

    [HttpGet("test")]
    public async Task<IActionResult> ErrorOrSuccess()
    {
        _logger
            .InformationLog(
                "Incoming request");

        var now = DateTime
            .UtcNow;

        // await a long operation
        await Task.Delay(1_000);

        if (now.Second > 10 &&
            now.Second <= 15)
        {
            _logger
                .ErrorLog("400");

            return new StatusCodeResult(
                StatusCodes.Status400BadRequest);
        }

        if (now.Second > 10 &&
            now.Second <= 30)
        {
            _logger
                .ErrorLog("503");

            return new StatusCodeResult(
                StatusCodes
                .Status503ServiceUnavailable);
        }

        if (now.Second > 10 &&
            now.Second <= 45)
        {
            _logger
                .ErrorLog("504");

            return new StatusCodeResult(
                StatusCodes
                .Status504GatewayTimeout);
        }

        if (now.Second > 10 &&
            now.Second <= 59)
        {
            _logger
                .ErrorLog("429");

            return new StatusCodeResult(
                StatusCodes
                .Status429TooManyRequests);
        }

        _logger
            .InformationLog("Success!");

        return new OkObjectResult(
            $"Request succeeded at {DateTime.UtcNow:HH:mm:ss}");
    }
}
