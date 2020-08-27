using Fanda.Core;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Serilog;
using System;
using System.Reflection;
using System.Threading.Tasks;

namespace Fanda.Auth
{
#pragma warning disable CS1591

    public class Program
    {
        public static async Task Main(string[] args)
        {
            Log.Logger = new LoggerConfiguration()
                .Enrich.FromLogContext()
                .WriteTo.Console()
                .CreateLogger();

            try
            {
                Log.Information("Fanda-Auth application starting up");
                var host = CreateHostBuilder(args).Build();
                await CreateAndRunTasks(host);
                host.Run();
                Log.Information("Fanda-Auth application shut down successfully");
            }
            catch (Exception ex)
            {
                Log.Fatal(ex, "Fanda-Auth application start-up failed");
            }
            finally
            {
                Log.CloseAndFlush();
            }
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .UseSerilog((builderContext, config) =>
                {
                    config
                        .MinimumLevel.Information()
                        .Enrich.FromLogContext()
                        .Enrich.WithMachineName()
                        .Enrich.WithProperty("Application", Assembly.GetExecutingAssembly().FullName)
                        .Enrich.WithEnvironmentUserName()
                        .Enrich.WithThreadId()
                        .WriteTo.Console();
                })
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });

        private static async Task CreateAndRunTasks(IHost host)
        {
            using (var scope = host.Services.CreateScope())
            {
                var services = scope.ServiceProvider;
                try
                {
                    var serviceProvider = services.GetRequiredService<IServiceProvider>();
                    //var configuration = services.GetRequiredService<IConfiguration>();
                    var options = services.GetRequiredService<IOptions<AppSettings>>();

                    SeedDefault seed = new SeedDefault(serviceProvider, options);
                    await seed.CreateFandaAppAsync();
                    await seed.CreateTenantAsync();
                }
                catch (Exception ex)
                {
                    var logger = services.GetRequiredService<ILogger<Program>>();
                    logger.LogError(ex, ex.Message);
                }
            }
        }
    }

#pragma warning restore CS1591
}
