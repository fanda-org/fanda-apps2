using AutoMapper;
using Fanda.Auth.Extensions;
using Fanda.Core;
using Fanda.Service.AutoMapperProfile;
using FandaAuth.Domain;
using FandaAuth.Service;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System.Reflection;

namespace Fanda.Auth
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
            services.AddCustomHealthChecks<AuthContext>();

            //services.Configure<AppSettings>(Configuration);
            //AppSettings appSettings = Configuration.Get<AppSettings>();
            var appSettings = services.ConfigureAppSettings(Configuration);

            //services.AddControllers();
            services.AddCustomControllers();

            //services.AddDbContext<AuthContext>(options =>
            //{
            //    options.UseMySql(Configuration.GetConnectionString("MySqlConnection"));
            //});
            services.AddCustomDbContext<AuthContext>(appSettings, Assembly.GetAssembly(typeof(AuthContext)).GetName().Name);
            services.AddCustomCors();
            services.AddAutoMapper(typeof(AuthProfile));
            services.AddJwtAuthentication(appSettings);
            services.AddSwagger("Fanda Authentication API");

            services.AddTransient<IEmailSender, EmailSender>();

            services.AddScoped<IApplicationRepository, ApplicationRepository>();
            services.AddScoped<ITenantRepository, TenantRepository>();
            services.AddScoped<IRoleRepository, RoleRepository>();
            services.AddScoped<IUserRepository, UserRepository>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env,
            AutoMapper.IConfigurationProvider autoMapperConfigProvider)
        {
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
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "Fanda Authentication API v1");
                c.RoutePrefix = "openapi";
            });
        }
    }
}
