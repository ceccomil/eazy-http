namespace CrudApi.Helpers;

public static class LogConfigExtensions
{
    internal class LoggerLevel
    {
        public string? Filter { get; set; }
        public LogLevel Level { get; set; }
    }

    public static ILoggingBuilder ConfigureLogging(
        this ILoggingBuilder logging)
    {
        using var sp = logging
            .Services
            .BuildServiceProvider();

        var conf = sp
            .GetRequiredService<IConfiguration>();

        var confList = new List<LoggerLevel>();

        conf
            .Bind(
                "LogLevels",
                confList);

        logging
            .ClearProviders()
            .AddCaptainLogger();

        foreach (var lc in confList)
        {
            logging
                .AddFilter(
                    lc.Filter,
                    lc.Level);
        }

        return logging;
    }
}
