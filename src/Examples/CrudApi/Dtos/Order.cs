namespace CrudApi.Dtos;

public record Order : OrderNoId
{
    public Guid Id { get; set; } = Guid.NewGuid();
}
