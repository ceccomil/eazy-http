namespace CrudApi.Client;

public class Order
{
    public Guid? Id { get; set; }
    public string? CustomerName { get; set; }
    public decimal Amount { get; set; }
    public string? Description { get; set; }

    public override string ToString() =>
        $"Order Id: {Id}{Environment.NewLine}" +
        $"From: {CustomerName}{Environment.NewLine}" +
        $"{Description} {Amount:N2}";
}
