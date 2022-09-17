namespace CrudApi.Controllers;

[ApiController]
[Route("[controller]")]
[Produces("application/json")]
[Authenticate]
public class OrdersController : Controller
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

    [HttpPost]
    public async Task<Order> Add(
        [FromBody] OrderNoId order)
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

    [HttpPut("{id:guid}")]
    public async Task<Order> Update(
        [FromRoute] Guid id,
        [FromBody] OrderNoId order)
    {
        _logger
            .InformationLog(
                "Request authorized Update");

        return await _db
            .Update(
                id,
                new()
                {
                    Id = id,
                    CustomerName = order.CustomerName,
                    Description = order.Description,
                    Amount = order.Amount
                });
    }

    [HttpPatch("{id:guid}")]
    public async Task<Order> ChangeAmount(
        [FromRoute] Guid id,
        [FromBody] OrderAmount oAmount)
    {
        _logger
            .InformationLog(
                "Request authorized ChangeAmount");

        var order = await _db.Get(id);
        order.Amount = oAmount.Amount;

        return await _db
            .Update(
                id,
                order);
    }

    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> Delete(
        [FromRoute] Guid id)
    {
        _logger
            .InformationLog(
                "Request authorized Delete");

        await _db
            .Delete(id);

        return Ok($"Order id {id} has been deleted!");
    }
}
