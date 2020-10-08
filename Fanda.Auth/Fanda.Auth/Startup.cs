using AutoMapper;
using Fanda.Core;
using Fanda.Core.Extensions;
using FandaAuth.Service.AutoMapperProfile;
using FandaAuth.Domain;
using FandaAuth.Service;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System.Reflection;

namespace Fanda.Auth
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
            services.AddCustomHealthChecks<AuthContext>();

            //AppSettings appSettings = Configuration.Get<AppSettings>();
            AppSettings appSettings = services.ConfigureAppSettings(Configuration);

            services.AddResponseCaching();
            services.AddCustomControllers();

            //services.AddDbContext<AuthContext>(options =>
            //{
            //    options.UseMySql(Configuration.GetConnectionString("MySqlConnection"));
            //});
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
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env,
            AutoMapper.IConfigurationProvider autoMapperConfigProvider, FandaAuth.Domain.AuthContext authDbContext)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            // migrate any database changes on startup (includes initial db creation)
            authDbContext.Database.Migrate();

            autoMapperConfigProvider.AssertConfigurationIsValid();

            //app.UseHttpsRedirection();

            app.UseCors("_MyAllowedOrigins");

            app.UseRouting();

            app.UseAuthentication();

            app.UseAuthorization();

            app.UseResponseCaching();

            app.UseEndpoints(endpoints =>
            {
                //endpoints.MapControllers();
                endpoints.MapControllerRoute(
                    name: "areaRoute",
                    pattern: "{area:exists}/{controller}/{action}",
                    defaults: new { action = "Index" });

                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller}/{action}/{id?}",
                    defaults: new { controller = "Home", action = "Index" });

                endpoints.MapControllerRoute(
                    name: "api",
                    pattern: "{controller}/{id?}");

                endpoints.MapHealthChecks("/health");
            });

            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("../swagger/v1/swagger.json", "Fanda Authentication API v1");
                c.RoutePrefix = "openapi";
            });
        }
    }
}