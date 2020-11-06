using System;
using System.Reflection;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using Serilog;

namespace Fanda.Accounting.Service
{
    // #pragma warning disable CS1591

    public class Program
    {
        public static void Main(string[] args)
        {
            Log.Logger = new LoggerConfiguration()
                .Enrich.FromLogContext()
                .WriteTo.Console()
                .CreateLogger();
            try
            {
                Log.Information("Fanda Accounting Service starting up");
                CreateHostBuilder(args).Build().Run();
                Log.Information("Fanda Accounting Service shut down successfully");
            }
            catch (Exception ex)
            {
                Log.Fatal(ex, "Fanda Accounting Service start-up failed");
            }
            finally
            {
                Log.CloseAndFlush();
            }
        }

        public static IHostBuilder CreateHostBuilder(string[] args)
        {
            return Host.CreateDefaultBuilder(args)
                .UseSerilog((builderContext, config) =>
                {
                    config
                        .MinimumLevel.Information()
                        .Enrich.FromLogContext()
                        .Enrich.WithMachineName()
                        .Enrich.WithProperty("Accounting", Assembly.GetExecutingAssembly().FullName)
                        .Enrich.WithEnvironmentUserName()
                        .Enrich.WithThreadId()
                        .WriteTo.Console();
                })
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                })
                .UseWindowsService();
        }
    }

    // #pragma warning restore CS1591
}