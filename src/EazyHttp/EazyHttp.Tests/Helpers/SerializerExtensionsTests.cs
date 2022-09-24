namespace EazyHttp.Tests.Helpers;

[Collection("Serialize rExtensions Tests")]
public class SerializerExtensionsTests
{
    public record TestClass
    {
        public Guid? Id { get; set; }
        public string Name { get; set; } = null!;
        public string Description { get; set; } = null!;
        public decimal Value { get; set; }
    }

    [Fact]
    public async Task SerializingContent()
    {
        // Arrange
        var options = new JsonSerializerOptions(
                        JsonSerializerDefaults.Web)
        {
            WriteIndented = true,
            DefaultIgnoreCondition = JsonIgnoreCondition
                .WhenWritingNull
        };

        var body = new TestClass()
        {
            Name = "Name",
            Description = "Description",
            Value = 100
        };

        // Act
        var content = await options
            .GetContentFromBody(
                body,
                Encoding.UTF8,
                CancellationToken.None)
            ?? throw new NullReferenceException();

        var value = await content
            .ReadAsStringAsync();

        var test = JsonSerializer
            .Deserialize<TestClass>(value, options)
            ?? throw new NullReferenceException();

        // Assert
        body
            .Should()
            .BeEquivalentTo(test);
    }
}
