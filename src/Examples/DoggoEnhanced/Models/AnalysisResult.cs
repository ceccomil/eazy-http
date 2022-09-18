namespace DoggoEnhanced.Models;

public class AnalysisResult
{
    [JsonPropertyName("requestId")]
    public Guid? Id { get; set; }

    public string ModelVersion { get; set; } = null!;

    public MetadataResult Metadata { get; set; } = null!;

    public DescriptionResult Description { get; set; } = null!;
}

public class Caption
{
    public string Text { get; set; } = null!;
    public decimal Confidence { get; set; }

    public override string ToString() =>
        $"{Text} [confidence: {Confidence:N2}]";
}

public class DescriptionResult
{
    public IEnumerable<string> Tags { get; set; } = null!;
    public IEnumerable<Caption> Captions { get; set; } = null!;
}

public class MetadataResult
{
    public int Height { get; set; }
    public int Width { get; set; }
    public string Format { get; set; } = null!;
}
