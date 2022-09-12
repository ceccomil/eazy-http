namespace CrudApi.Controllers;

[ApiController]
[Route("[controller]")]
[Produces("application/json")]
[Authenticate]
public class OrdersController
{
    private readonly ICaptainLogger _logger;
    private readonly IDbService _db;

    public OrdersController(
        ICaptainLogger<OrdersController> logger,
        IDbService db)
    {
        _logger = logger;
        _db = db;
    }

    [HttpGet]
    public async Task<IEnumerable<Order>> GetAll()
    {
        _logger
            .InformationLog(
                "Request authorized GetAll");

        return await _db.GetAll();
    }

    [HttpGet("{id:guid}")]
    public async Task<Order> Get(
        [FromRoute] Guid id)
    {
        _logger
            .InformationLog(
                "Request authorized Get");

        return await _db.Get(id);
    }

    [HttpPost]
    public async Task<Order> Add(
        [FromBody] OrderAdd order)
    {
        _logger
            .InformationLog(
                "Request authorized Add");

        return await _db
            .Add(new()
            {
                CustomerName = order.CustomerName,
                Amount = order.Amount,
                Description = order.Description
            });
    }
}
