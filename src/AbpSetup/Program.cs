using System;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Serilog;
using Serilog.Events;

namespace AbpSetup;

public class Program
{
    public async static Task<int> Main(string[] args)
    {
        Log.Logger = new LoggerConfiguration()
#if DEBUG
            .MinimumLevel.Debug()
#else
            .MinimumLevel.Information()
#endif
            .MinimumLevel.Override("Microsoft", LogEventLevel.Information)
            .Enrich.FromLogContext()
            .WriteTo.Async(c => c.File("Logs/logs.txt"))
            .WriteTo.Async(c => c.Console())
            .CreateLogger();

        try
        {
            Log.Information("Starting console host.");
            var builder = ConsoleApp.CreateBuilder(args);
            builder.ConfigureServices(services =>
            {
                services.AddHostedService<AbpSetupHostedService>();
                services.AddApplication<AbpSetupModule>();
            });
            var app = builder.Build();
            //app.AddRootCommand(() => new CreateCommand(app.Services.GetRequiredService<HelloWorldService>()));
            app.AddCommands<CreateCommand>();
            app.AddCommands<TestCommand>();
            await app.RunAsync();
            return 0;
        }
        catch (Exception ex)
        {
            Log.Fatal(ex, "Host terminated unexpectedly!");
            return 1;
        }
        finally
        {
            Log.CloseAndFlush();
        }
    }
}
