using DotNetEnv;
using DotNetEnv.Configuration;
using Serilog;
using Serilog.Events;

namespace NotificationCalendar.Api.Hosting;

public class ApiHost
{
    public static int Run<T>(string[] args) where T : class
    {
        Log.Logger = new LoggerConfiguration()
            .MinimumLevel.Debug()
            .MinimumLevel.Override("Microsoft", LogEventLevel.Information)
            .Enrich.FromLogContext()
            .Enrich.WithProperty("service", typeof(T).Name)
            .WriteTo.Console()
            .CreateLogger();

        try
        {
            Log.Information("Starting host");
            CreateDefaultBuilder<T>(args).Build().Run();

            return 0;
        }
        catch (Exception ex)
        {
            Log.Fatal(ex, "Host terminated unexpectedly");

            return 1;
        }
        finally
        {
            Log.CloseAndFlush();
        }
    }

    public static IHostBuilder CreateDefaultBuilder<T>(string[] args) where T : class
    {
        AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);
        AppContext.SetSwitch("System.Net.Http.EnableActivityPropagation", false);

        IConfigurationRoot builtConfig = new ConfigurationBuilder()
            .AddJsonFile("appsettings.json", true, true)
            .AddJsonFile("appsettings." + Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") + ".json", true)
            .AddDotNetEnv(".env", LoadOptions.TraversePath())
            .Build();

        return Host.CreateDefaultBuilder(args)
            .ConfigureAppConfiguration(config => config.AddConfiguration(builtConfig))
            .UseSerilog()
            .ConfigureWebHostDefaults(webBuilder => webBuilder.UseStartup<T>());
    }
}
