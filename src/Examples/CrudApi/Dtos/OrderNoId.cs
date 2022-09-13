namespace CrudApi.Dtos;

public record OrderNoId : OrderAmount
{
    public string CustomerName { get; set; } = null!;
    public string Description { get; set; } = null!;
}
