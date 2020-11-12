using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;

using Serilog;

using System.Reflection;

namespace Fanda.ApiGateway
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
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
                        .Enrich.WithProperty("Application", Assembly.GetExecutingAssembly().FullName)
                        .Enrich.WithEnvironmentUserName()
                        .Enrich.WithThreadId()
                        .WriteTo.Console();
                })
                .ConfigureAppConfiguration((hostingContext, config) =>
                {
                    config
                        .SetBasePath(hostingContext.HostingEnvironment.ContentRootPath)
                        .AddJsonFile("ocelot.json", false, true);
                })
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder
                        //.UseUrls("http://0.0.0.0:80")
                        .UseStartup<Startup>();
                })
                .UseWindowsService();
        }
    }
}