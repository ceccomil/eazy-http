using System.Text.Json.Serialization;

namespace CryptoPrices;

public class ResultDto
{
    public StatusDto Status { get; set; } = null!;
    public IEnumerable<DataDto> Data { get; set; } = null!;
}

public class StatusDto
{
    public double Elapsed { get; set; }
    public DateTimeOffset Timestamp { get; set; }
}

public class DataDto
{
    public Guid Id { get; set; }
    public string Slug { get; set; } = null!;
    public string Symbol { get; set; } = null!;
    public MetricsDto Metrics { get; set; } = null!;
}

public class MetricsDto
{
    [JsonPropertyName("market_data")]
    public MarketDataDto MarketData { get; set; } = null!;
}

public class MarketDataDto
{
    [JsonPropertyName("price_usd")]
    public decimal PriceUsd { get; set; }
}
