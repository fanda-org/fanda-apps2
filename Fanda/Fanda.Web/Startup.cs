using AutoMapper;
using Fanda.Infrastructure.Helpers;
using Fanda.Infrastructure;
using Fanda.Infrastructure.Extensions;
using Fanda.Shared;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.DataProtection.AuthenticatedEncryption;
using Microsoft.AspNetCore.DataProtection.AuthenticatedEncryption.ConfigurationModel;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.ResponseCompression;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Serilog;
using System;
using System.IO;
using System.Linq;
using System.Net.Mime;
using System.Reflection;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using Microsoft.AspNetCore.SpaServices.AngularCli;

[assembly: ApiController]
namespace Fanda.Web
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.ConfigureStartupServices<FandaContext>(Configuration, Assembly.GetAssembly(typeof(Startup)).GetName().Name);

            #region Repositories
            services.AddTransient<IEmailSender, EmailSender>();

            //services.AddScoped<ISerialNumberRepository, SerialNumberRepository>();
            //services.AddScoped<IUserRepository, UserRepository>();
            //services.AddScoped<IOrganizationRepository, OrganizationRepository>();
            services.AddScoped<IUnitRepository, UnitRepository>();
            #endregion

            #region Angular SPA
            services.AddControllersWithViews();
                //.AddRazorRuntimeCompilation();
            // In production, the Angular files will be served from this directory
            services.AddSpaStaticFiles(configuration =>
            {
               configuration.RootPath = "ClientApp/dist";
            });
            #endregion
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env,
            AutoMapper.IConfigurationProvider autoMapperConfigProvider)
        {
            app.ConfigureStartup(env, autoMapperConfigProvider);

            #region Angular SPA
            app.UseStaticFiles();
            if (!env.IsDevelopment())
            {
               app.UseSpaStaticFiles();
            }
            #endregion

            // app.UseEndpoints(endpoints =>
            // {
            //     //endpoints.MapControllerRoute(
            //     //    name: "default",
            //     //    pattern: "{controller}/{action=Index}/{id?}");
            // });

            #region Angular SPA
            app.UseSpa(spa =>
            {
               // To learn more about options for serving an Angular SPA from ASP.NET Core,
               // see https://go.microsoft.com/fwlink/?linkid=864501

               spa.Options.SourcePath = "ClientApp";

               if (env.IsDevelopment())
               {
                   spa.UseAngularCliServer(npmScript: "start");
               }
            });
            #endregion
        }
    }
}
