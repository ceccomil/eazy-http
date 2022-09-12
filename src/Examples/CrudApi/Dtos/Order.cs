namespace CrudApi.Dtos;

public record Order : OrderAdd
{
    public Guid Id { get; set; } = Guid.NewGuid();
}
