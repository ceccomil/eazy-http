using Microsoft.AspNetCore.Mvc;
using System.Text;

namespace IntegrationApi.Controllers;

[ApiController]
[Route("integration-test")]
public sealed class TestController : ControllerBase
{
    private static readonly string[] Summaries =
    [
        "Freezing", "Bracing",
        "Chilly", "Cool", "Mild",
        "Warm", "Balmy", "Hot",
        "Sweltering", "Scorching"
    ];

    [HttpGet("forecast")]
    public IEnumerable<WeatherForecast> Get() => Enumerable
        .Range(1, 5)
        .Select(index => new WeatherForecast
        {
            Date = DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
            TemperatureC = Random.Shared.Next(-20, 55),
            Summary = Summaries[Random.Shared.Next(Summaries.Length)]
        })
    .ToArray();

    [HttpGet("download")]
    public IActionResult GetADummyFile()
    {
        var sb = new StringBuilder();

        for (var i = 0; i < 200_000; i++)
        {
            sb.AppendLine("This is a dummy file.");
        }

        var content = Encoding
            .UTF8
            .GetBytes(sb.ToString());

        return File(
            content,
            "text/plain",
            "sample.txt");
    }
}
