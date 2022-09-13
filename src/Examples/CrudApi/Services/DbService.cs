namespace CrudApi.Services;

public interface IDbService : IDisposable
{
    LiteDatabase Database { get; }
    ILiteCollection<Order> Orders { get; }

    Task<Order> Add(Order order);
    Task<Order> Get(Guid id);
    Task<Order> Update(Guid id, Order order);
    Task Delete(Guid id);
    Task<IEnumerable<Order>> GetAll();
}

public class DbService : IDbService
{
    private bool _disposed = false;

    public LiteDatabase Database { get; }
    public ILiteCollection<Order> Orders { get; }

    public DbService(
        IConfiguration configuration)
    {
        Database = new(
            configuration["DbFile"]);

        Orders = Database
            .GetCollection<Order>(
                "Orders");
    }

    ~DbService() => Dispose(false);

    private void Dispose(bool disposing)
    {
        if (_disposed)
        {
            return;
        }

        if (disposing)
        {
            Database.Dispose();
        }

        _disposed = true;
    }

    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    public Task<Order> Add(Order order)=> Task.Run(() =>
    {
        if (Orders.FindById(order.Id) is not null)
        {
            throw new ApplicationException(
                $"Order Id: {order.Id}, already exists!");
        }

        _ = Orders.Insert(order);

        return Orders.FindById(order.Id);
    });

    public Task<Order> Get(Guid id) => Task.Run(() =>
    {
        if (Orders.FindById(id) is not Order order)
        {
            throw new ApplicationException(
                $"Order Id: {id}, has not been found!");
        }

        return order;
    });

    public Task<Order> Update(Guid id, Order order) => Task.Run(() =>
    {
        if (Orders.FindById(id) is not Order)
        {
            throw new ApplicationException(
                $"Order Id: {id}, has not been found!");
        }

        _ = Orders.Update(id, order);

        return Orders.FindById(id);
    });

    public Task Delete(Guid id) => Task.Run(() =>
    {
        if (Orders.FindById(id) is not Order order)
        {
            return;
        }

        Orders.Delete(id);
    });

    public Task<IEnumerable<Order>> GetAll() => Task
        .FromResult(Orders
            .FindAll());
}
