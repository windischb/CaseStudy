using Microsoft.AspNetCore.Server.Kestrel.Core;
using Serilog;
using Serilog.Sinks.SystemConsole.Themes;
using System.Diagnostics;
using System.Net;
using CaseStudy.Api.Helpers;

namespace CaseStudy.Api;

public class Program
{

    public static async Task Main(string[] args)
    {
        Log.Logger = new LoggerConfiguration()
            .WriteTo.Console()
            .CreateBootstrapLogger();

        try
        {
            Log.Information("Starting web host");
            var host = CreateHostBuilder(args).Build();
            await Infrastructure.DbMigrator.Migrate(host.Services);
            await host.RunAsync();
        }
        catch (Exception ex)
        {
            Log.Fatal(ex, "Host terminated unexpectedly");
        }
        finally
        {
            await Log.CloseAndFlushAsync();
        }
    }

    public static IHostBuilder CreateHostBuilder(string[] args) =>
        Host.CreateDefaultBuilder(args)
            .ConfigureAppConfiguration((hostingContext, config) =>
            {
                if (!Debugger.IsAttached)
                {
                    config.AddJsonFile(PathHelper.GetFullPath("config.json"), optional: true, reloadOnChange: true);
                }
                config.AddEnvironmentVariables();
            })
            .UseSerilog((hostingContext, loggerConfiguration) =>
                {
                    var startUpConfiguration = hostingContext.Configuration.Get<StartUpConfiguration>();
                    foreach (var (key, value) in startUpConfiguration.Logging.LogLevels)
                    {
                        if (key.Equals("default", StringComparison.OrdinalIgnoreCase) || key.Equals("*", StringComparison.OrdinalIgnoreCase))
                        {
                            loggerConfiguration.MinimumLevel.Is(value);
                        }
                        else
                        {
                            loggerConfiguration.MinimumLevel.Override(key, value);
                        }

                    }

                    loggerConfiguration.WriteTo.Console(theme: AnsiConsoleTheme.Code);
                }
            )
            .ConfigureWebHostDefaults(webBuilder =>
            {
                webBuilder.UseKestrel(ConfigureKestrel).UseStartup<Startup>();
            });

    private static void ConfigureKestrel(WebHostBuilderContext context, KestrelServerOptions serverOptions)
    {

        var config = context.Configuration.Get<StartUpConfiguration>()!;
        var listenIp = IPAddress.Parse(config.ListeningIP);

        serverOptions.Listen(listenIp, config.HttpsPort, listenOptions =>
        {
            listenOptions.Protocols = HttpProtocols.Http1AndHttp2;
            listenOptions.UseHttps(PathHelper.GetFullPath(config.HttpsCertPath), config.HttpsCertPassword);

        });

    }

}