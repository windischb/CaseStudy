using System.ComponentModel;
using Serilog.Events;

namespace CaseStudy.Api;
public class StartUpConfiguration
{

    [Description("Limit API to listen only on specified IP's")]
    public string ListeningIP { get; set; } = "0.0.0.0";

    [Description("API Listening HTTPS Port")]
    public int HttpsPort { get; set; } = 443;

    [Description("Path to the SSL Certificate used for HTTPS")]
    public string HttpsCertPath { get; set; } = "localhost.pfx";

    [Description("Password for the SSL Certificate used by HTTPS")]
    public string HttpsCertPassword { get; set; } = "ABC12abc";

    public Logging Logging { get; set; } = new();

    public DatabaseConfiguration DbSettings { get; } = new();
    public RedisConfiguration RedisSettings { get; } = new();
}

public class Logging
{

    private Dictionary<string, LogEventLevel>? _logLevels;

    public Dictionary<string, LogEventLevel> LogLevels
    {
        get => _logLevels ??= GetDefaultLoggingLevels();
        set => _logLevels = value;
    }



    internal static Dictionary<string, LogEventLevel> GetDefaultLoggingLevels()
    {
        return new Dictionary<string, LogEventLevel>(StringComparer.OrdinalIgnoreCase)
        {
            ["Default"] = LogEventLevel.Warning,
            ["Microsoft"] = LogEventLevel.Warning,
            ["Microsoft.Hosting.Lifetime"] = LogEventLevel.Information,
            ["System"] = LogEventLevel.Warning,
            ["Microsoft.AspNetCore.Authentication"] = LogEventLevel.Warning,
            ["Microsoft.AspNetCore.Http.Connections"] = LogEventLevel.Debug
        };
    }
}

public class DatabaseConfiguration
{
    public string ConnectionString { get; set; } = "Host=127.0.0.1;Database=CaseStudy;Username=postgres;Password=postgres";
}

public class RedisConfiguration
{
    public string ConnectionString { get; set; } = "127.0.0.1:6379";
}
