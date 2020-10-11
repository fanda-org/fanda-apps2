using AutoMapper;
using Fanda.Core;
using Fanda.Core.Extensions;
using Fanda.Domain.Context;
using Fanda.Service;
using Fanda.Service.ApiClients;
using Fanda.Service.AutoMapperProfiles;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SpaServices.AngularCli;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Reflection;

[assembly: ApiController]

namespace Fanda.Web
{
    public class Startup
    {
        private IConfiguration Configuration { get; }
        private IWebHostEnvironment Env { get; }

        public Startup(IConfiguration configuration, IWebHostEnvironment env)
        {
            Configuration = configuration;
            Env = env;
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            //services.ConfigureStartupServices<FandaContext>(Configuration, Assembly.GetAssembly(typeof(Startup)).GetName().Name);

            #region Startup configure services

            services.AddCustomHealthChecks<FandaContext>();

            //services.Configure<AppSettings>(Configuration);
            //AppSettings appSettings = Configuration.Get<AppSettings>();

            AppSettings appSettings = services.ConfigureAppSettings(Configuration);

            //services.AddControllers();
            services.AddCustomControllers();
            services.AddResponseCaching();

            //services.AddDbContext<AuthContext>(options =>
            //{
            //    options.UseMySql(Configuration.GetConnectionString("MySqlConnection"));
            //});
            services.AddCustomDbContext<FandaContext>(appSettings, Assembly.GetAssembly(typeof(FandaContext)).GetName().Name, Env.IsDevelopment())
                .AddCustomCors()
                .AddAutoMapper(typeof(AutoMapperProfile))
                .AddSwagger("Fanda Application API");
            services.AddJwtAuthentication(appSettings.FandaSettings.Secret);

            #endregion Startup configure services

            #region Other services

            services.AddHttpClient("auth_api", c =>
            {
                c.BaseAddress = new Uri(appSettings.AuthService.Url);
            })
               .AddTypedClient(c => Refit.RestService.For<IAuthClient>(c));

            #endregion Other services

            #region DI Repositories and services

            services.AddTransient<IEmailSender, EmailSender>();

            services.AddScoped<IOrganizationRepository, OrganizationRepository>();
            services.AddScoped<IPartyCategoryRepository, PartyCategoryRepository>();
            //services.AddScoped<ISerialNumberRepository, SerialNumberRepository>();
            //services.AddScoped<IUserRepository, UserRepository>();
            //services.AddScoped<IUnitRepository, UnitRepository>();

            #endregion DI Repositories and services

            #region Angular SPA

            services.AddControllersWithViews();
            //.AddRazorRuntimeCompilation();
            // In production, the Angular files will be served from this directory
            services.AddSpaStaticFiles(configuration =>
            {
                configuration.RootPath = "ClientApp/dist";
            });

            #endregion Angular SPA
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env,
            AutoMapper.IConfigurationProvider autoMapperConfigProvider)
        {
            //app.ConfigureStartup(env, autoMapperConfigProvider);

            #region Startup configure

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            autoMapperConfigProvider.AssertConfigurationIsValid();

            //app.UseHttpsRedirection();

            app.UseCors("_MyAllowedOrigins");

            app.UseRouting();

            app.UseAuthentication();

            app.UseAuthorization();
            app.UseResponseCaching();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
                endpoints.MapHealthChecks("/health");
            });

            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("../swagger/v1/swagger.json", "Fanda Application API v1");
                c.RoutePrefix = "openapi";
            });

            #endregion Startup configure

            #region Angular SPA

            app.UseStaticFiles();
            if (!env.IsDevelopment())
            {
                app.UseSpaStaticFiles();
            }

            #endregion Angular SPA

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

            #endregion Angular SPA
        }
    }
}
