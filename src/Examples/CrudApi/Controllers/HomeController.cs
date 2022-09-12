using System.Net;

namespace CrudApi.Controllers;

[ApiController]
[Route("[controller]")]
public class HomeController : Controller
{
    [HttpGet]
    public IActionResult Status() => new ContentResult
    {
        ContentType = "text/html",
        StatusCode = (int)HttpStatusCode.OK,
        Content = "Crud API - OK"
    };
}
