using AutoMapper;
using Fanda.Authentication.Domain;
using Fanda.Authentication.Repository;
using Fanda.Authentication.Repository.AutoMapperProfile;
using Fanda.Core;
using Fanda.Core.Extensions;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Reflection;
using System.Threading.Tasks;
using IConfigurationProvider = AutoMapper.IConfigurationProvider;

[assembly: ApiController]

namespace Fanda.Authentication.Service
{
    public class Startup
    {
        public Startup(IConfiguration configuration, IWebHostEnvironment env)
        {
            Configuration = configuration;
            Env = env;
        }

        private IConfiguration Configuration { get; }
        private IWebHostEnvironment Env { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddCustomHealthChecks<AuthContext>();

            //AppSettings appSettings = Configuration.Get<AppSettings>();
            var appSettings = services.ConfigureAppSettings(Configuration);

            services.AddCustomControllers();
            //services.AddResponseCaching();

            services.AddCustomDbContext<AuthContext>(appSettings,
                    Assembly.GetAssembly(typeof(AuthContext)).GetName().Name, Env.IsDevelopment())
                .AddCustomCors()
                .AddAutoMapper(typeof(AuthProfile))
                .AddSwagger("Fanda Authentication API");

            services.AddJwtAuthentication(appSettings.FandaSettings.Secret);

            #region DI - Repositories and services

            services.AddTransient<IEmailSender, EmailSender>();

            services.AddScoped<IApplicationRepository, ApplicationRepository>();
            services.AddScoped<ITenantRepository, TenantRepository>();
            services.AddScoped<IRoleRepository, RoleRepository>();
            services.AddScoped<IAuthRepository, AuthRepository>();
            services.AddScoped<IUserRepository, UserRepository>();

            #endregion DI - Repositories and services
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public async void Configure(IApplicationBuilder app, IWebHostEnvironment env,
            IConfigurationProvider autoMapperConfigProvider, AuthContext authDbContext, IHost host)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            // migrate any database changes on startup (includes initial db creation)
            authDbContext.Database.Migrate();
            await SeedDataAsync(host);
            autoMapperConfigProvider.AssertConfigurationIsValid();

            //app.UseHttpsRedirection();

            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("../swagger/v1/swagger.json", "Fanda Authentication API v1");
                c.RoutePrefix = "openapi";
            });

            app.UseCors("_MyAllowedOrigins");
            app.UseRouting();
            app.UseAuthentication();
            app.UseAuthorization();
            //app.UseResponseCaching();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
                //.RequireCors("_MyAllowedOrigins");
                endpoints.MapHealthChecks("/health");

                //endpoints.MapControllerRoute(
                //    "areaRoute",
                //    "{area:exists}/{controller}/{action}",
                //    new { action = "Index" });
                ////.RequireCors("_MyAllowedOrigins");

                //endpoints.MapControllerRoute(
                //    "default",
                //    "{controller}/{action}/{id?}",
                //    new { controller = "Home", action = "Index" });
                ////.RequireCors("_MyAllowedOrigins");

                //endpoints.MapControllerRoute(
                //    "api",
                //    "api/{controller}/{id?}");
                ////.RequireCors("_MyAllowedOrigins");
            });
        }

        private static async Task SeedDataAsync(IHost host)
        {
            using (var scope = host.Services.CreateScope())
            {
                var services = scope.ServiceProvider;
                try
                {
                    var serviceProvider = services.GetRequiredService<IServiceProvider>();
                    //var configuration = services.GetRequiredService<IConfiguration>();
                    var options = services.GetRequiredService<IOptions<AppSettings>>();

                    var seed = new SeedDefault(serviceProvider /*, options*/);
                    await seed.CreateFandaAppAsync();
                    await seed.CreateTenantAsync();
                }
                catch (Exception ex)
                {
                    var logger = services.GetRequiredService<ILogger<Startup>>();
                    logger.LogError(ex, ex.Message);
                }
            }
        }
    }
}