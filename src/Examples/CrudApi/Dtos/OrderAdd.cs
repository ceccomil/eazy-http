namespace CrudApi.Dtos;

public record OrderAdd
{
    public string CustomerName { get; set; } = null!;
    public decimal Amount { get; set; }
    public string Description { get; set; } = null!;
}
