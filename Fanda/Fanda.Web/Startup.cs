using AutoMapper;
using Fanda.Core;
using Fanda.Core.Extensions;
using Fanda.Domain.Context;
using Fanda.Service;
using Fanda.Service.AutoMapperProfiles;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SpaServices.AngularCli;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System.Reflection;

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
            //services.ConfigureStartupServices<FandaContext>(Configuration, Assembly.GetAssembly(typeof(Startup)).GetName().Name);

            #region Startup configure services

            services.AddCustomHealthChecks<FandaContext>();

            //services.Configure<AppSettings>(Configuration);
            //AppSettings appSettings = Configuration.Get<AppSettings>();

            AppSettings appSettings = services.ConfigureAppSettings(Configuration);

            //services.AddControllers();
            services.AddCustomControllers();

            //services.AddDbContext<AuthContext>(options =>
            //{
            //    options.UseMySql(Configuration.GetConnectionString("MySqlConnection"));
            //});
            services.AddCustomDbContext<FandaContext>(appSettings,
                Assembly.GetAssembly(typeof(FandaContext)).GetName().Name)
                .AddCustomCors()
                .AddAutoMapper(typeof(AutoMapperProfile))
                .AddSwagger("Fanda Application API");
            services.AddJwtAuthentication(appSettings);

            #endregion Startup configure services

            #region Repositories

            services.AddTransient<IEmailSender, EmailSender>();

            //services.AddScoped<ISerialNumberRepository, SerialNumberRepository>();
            //services.AddScoped<IUserRepository, UserRepository>();
            //services.AddScoped<IOrganizationRepository, OrganizationRepository>();
            services.AddScoped<IUnitRepository, UnitRepository>();

            #endregion Repositories

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
