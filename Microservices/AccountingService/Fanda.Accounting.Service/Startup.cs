using AutoMapper;
using Fanda.Accounting.Domain.Context;
using Fanda.Accounting.Repository;
using Fanda.Accounting.Repository.ApiClients;
using Fanda.Accounting.Repository.AutoMapperProfiles;
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
using Refit;
using System;
using System.Reflection;
using IConfigurationProvider = AutoMapper.IConfigurationProvider;

[assembly: ApiController]

namespace Fanda.Accounting.Service
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
            //services.ConfigureStartupServices<FandaContext>(Configuration, Assembly.GetAssembly(typeof(Startup)).GetName().Name);

            #region Startup configure services

            services.AddCustomHealthChecks<AcctContext>();

            //services.Configure<AppSettings>(Configuration);
            //AppSettings appSettings = Configuration.Get<AppSettings>();

            var appSettings = services.ConfigureAppSettings(Configuration);

            //services.AddControllers();
            services.AddCustomControllers();
            //services.AddResponseCaching();

            //services.AddDbContext<AuthContext>(options =>
            //{
            //    options.UseMySql(Configuration.GetConnectionString("MySqlConnection"));
            //});
            services.AddCustomDbContext<AcctContext>(appSettings,
                    Assembly.GetAssembly(typeof(AcctContext)).GetName().Name, Env.IsDevelopment())
                .AddCustomCors()
                .AddAutoMapper(typeof(AutoMapperProfile))
                .AddSwagger("Fanda Accounting API");
            services.AddJwtAuthentication(appSettings.FandaSettings.Secret);

            #endregion Startup configure services

            #region Other services

            services.AddHttpClient("auth_api", c =>
                {
                    c.BaseAddress = new Uri(appSettings.Services.AuthServiceUrl);
                })
                .AddTypedClient(c => RestService.For<IAuthClient>(c));

            #endregion Other services

            #region DI Repositories and services

            services.AddTransient<IEmailSender, EmailSender>();

            services.AddScoped<IOrganizationRepository, OrganizationRepository>();
            services.AddScoped<IPartyCategoryRepository, PartyCategoryRepository>();
            services.AddScoped<ILedgerGroupRepository, LedgerGroupRepository>();
            services.AddScoped<ILedgerRepository, LedgerRepository>();
            services.AddScoped<IJournalRepository, JournalRepository>();
            services.AddScoped<IOrgUserRepository, OrgUserRepository>();
            services.AddScoped<IAccountYearRepository, AccountYearRepository>();
            //services.AddScoped<IOrgRoleRepository, OrgRoleRepository>();
            //services.AddScoped<ISerialNumberRepository, SerialNumberRepository>();
            //services.AddScoped<IUserRepository, UserRepository>();
            //services.AddScoped<IUnitRepository, UnitRepository>();

            #endregion DI Repositories and services
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env,
            IConfigurationProvider autoMapperConfigProvider, AcctContext acctDbContext, IHost host)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("../swagger/v1/swagger.json", "Fanda Application API v1");
                c.RoutePrefix = "openapi";
            });

            // migrate any database changes on startup (includes initial db creation)
            acctDbContext.Database.Migrate();
            SeedDataAsync(host);
            autoMapperConfigProvider.AssertConfigurationIsValid();

            app.UseHttpsRedirection();

            app.UseRouting();
            app.UseCors("_MyAllowedOrigins");
            app.UseAuthentication();
            app.UseAuthorization();
            //app.UseResponseCaching();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
                //.RequireCors("_MyAllowedOrigins");
                endpoints.MapHealthChecks("/health");
            });
        }

        private static void SeedDataAsync(IHost host)
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