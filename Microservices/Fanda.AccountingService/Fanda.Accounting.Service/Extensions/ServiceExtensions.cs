using System;
using System.IO;
using System.Linq;
using System.Net.Mime;
using System.Reflection;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using Fanda.Core;
using Fanda.Core.Extensions;
using global::AutoMapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.DataProtection.AuthenticatedEncryption;
using Microsoft.AspNetCore.DataProtection.AuthenticatedEncryption.ConfigurationModel;
using Microsoft.AspNetCore.Hosting;

//using Microsoft.AspNetCore.ResponseCompression;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;

namespace Fanda.Service.Extensions
{
    public static class ServiceExtensions
    {
        public static void ConfigureStartupServices<TDbContext>(this IServiceCollection services,
            IConfiguration configuration, string migrationAssemblyName)
            where TDbContext : DbContext
        {
            #region Health checks

            services.AddHealthChecks()
                .AddDbContextCheck<TDbContext>();

            #endregion Health checks

            #region AppSettings

            services.Configure<AppSettings>(configuration);
            AppSettings appSettings = configuration.Get<AppSettings>();

            #endregion AppSettings

            #region DbContext

            services.AddFandaDbContextPool<TDbContext>(appSettings, migrationAssemblyName);

            #endregion DbContext

            #region CORS

            services.AddCors(options =>
            {
                var urls = new[]
                {
                    "http://localhost:5100",
                    "http://localhost:5200",
                    "http://localhost:4200",
                   //Configuration["Fanda.Web.Url"],
                   //Configuration["Fanda.Ng.Url"]
                };
                options.AddPolicy("_MyAllowedOrigins", builder =>
                {
                    builder.WithOrigins(urls)
                    .AllowAnyHeader()
                    .AllowAnyMethod();
                    //.AllowCredentials();
                });
                //options.AddPolicy("OrderApp",
                //    policy => policy.AllowAnyOrigin()
                //    .AllowAnyHeader()
                //    .AllowAnyMethod());
            });

            //services.AddCors();
            //options =>
            //{
            //    options.AddPolicy("AllowAll", builder =>
            //    {
            //        builder
            //        .AllowAnyOrigin()
            //        .AllowAnyMethod()
            //        .AllowAnyHeader();
            //        //.AllowCredentials();
            //    });

            //var urls = new[]
            //{
            //    Configuration["Order.Web.Url"],
            //    Configuration["Order.Ng.Url"]
            //};
            //options.AddPolicy("_MyAllowedOrigins", builder =>
            //{
            //    builder.WithOrigins(urls)
            //    .AllowAnyHeader()
            //    .AllowAnyMethod();
            //});
            //});

            #endregion CORS

            #region Commented - Response Caching

            //services.AddResponseCaching();

            #endregion Commented - Response Caching

            #region Response compression

            //services.AddResponseCompression(options =>
            //{
            //    options.Providers.Add<GzipCompressionProvider>();
            //    options.MimeTypes = ResponseCompressionDefaults.MimeTypes.Concat(new[] { "image/svg+xml" });
            //});
            //services.Configure<GzipCompressionProviderOptions>(options =>
            //{
            //    options.Level = System.IO.Compression.CompressionLevel.Fastest;
            //});

            #endregion Response compression

            #region Commented - DistributedMemoryCache, DataProtection and Session

            // Adds a default in-memory implementation of IDistributedCache.
            //services.AddDistributedMemoryCache(options =>
            //{
            //    //options.ExpirationScanFrequency = TimeSpan.FromMinutes(appSettings.Cache.ExpirationMinute);
            //    // Default size limit of 200 MB
            //    //options.SizeLimit = appSettings.Cache.SizeLimitMB * 1024L * 1024L;
            //});

            //Distributed Cache
            //services.AddSingleton<IWebCache, WebCache>();

            #region DataProtection

            services.AddDataProtection()
                .UseCryptographicAlgorithms(new AuthenticatedEncryptorConfiguration()
                {
                    EncryptionAlgorithm = EncryptionAlgorithm.AES_256_CBC,
                    ValidationAlgorithm = ValidationAlgorithm.HMACSHA256
                });

            #endregion DataProtection

            //services.AddSession(options =>
            //{
            //    //options.Cookie.HttpOnly = false;
            //    //options.Cookie.Name = ".Fanda.Session";
            //    //options.Cookie.SecurePolicy = CookieSecurePolicy.SameAsRequest;
            //    //options.Cookie.SameSite = SameSiteMode.Lax;
            //    //options.IdleTimeout = TimeSpan.FromMinutes(20);

            //    //options.Cookie.IsEssential = true;
            //    //options.Cookie.Path = "/";
            //});

            #endregion Commented - DistributedMemoryCache, DataProtection and Session

            #region AutoMapper

            services.AddAutoMapper(typeof(AutoMapperProfiles.AutoMapperProfile));

            #endregion AutoMapper

            #region Commented - Authorization

            //services.AddAuthorization();

            #endregion Commented - Authorization

            #region JWT Authentication

            var key = Encoding.ASCII.GetBytes(appSettings.FandaSettings.Secret);
            services.AddAuthentication(x =>
            {
                x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(x =>
            {
                //x.Events = new JwtBearerEvents
                //{
                //    OnTokenValidated = (context) =>
                //    {
                //        var userService = context.HttpContext.RequestServices.GetRequiredService<IUserRepository>();
                //        Guid userId = new Guid(context.Principal.Identity.Name);
                //        var user = userService.GetByIdAsync(userId).Result;
                //        if (user == null)
                //        {
                //            // return unauthorized if user no longer exists
                //            context.Fail("Unauthorized");
                //        }
                //        return Task.CompletedTask;
                //    }
                //};
                x.RequireHttpsMetadata = false;
                x.SaveToken = true;
                x.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    // set clockskew to zero so tokens expire exactly at token expiration time (instead of 5 minutes later)
                    ClockSkew = TimeSpan.Zero
                };
            });

            #endregion JWT Authentication

            // #region Repositories
            // services.AddTransient<IEmailSender, EmailSender>();

            // services.AddScoped<ISerialNumberRepository, SerialNumberRepository>();
            // services.AddScoped<IUserRepository, UserRepository>();
            // services.AddScoped<IOrganizationRepository, OrganizationRepository>();
            // services.AddScoped<IUnitRepository, UnitRepository>();
            // #endregion

            #region AddControllers

            services.AddControllers()
                .ConfigureApiBehaviorOptions(options =>
                {
                    options.InvalidModelStateResponseFactory = actionContext =>
                    {
                        var result = new ValidationFailedResult(actionContext.ModelState);
                        // add `using using System.Net.Mime;` to resolve MediaTypeNames
                        result.ContentTypes.Add(MediaTypeNames.Application.Json);
                        result.ContentTypes.Add(MediaTypeNames.Application.Xml);
                        return result;
                    };
                })
                .AddXmlSerializerFormatters()
                .AddJsonOptions(options =>
                {
                    //options.SerializerSettings.ContractResolver = new DefaultContractResolver();    //new CamelCasePropertyNamesContractResolver();
                    //options.SerializerSettings.Converters.Add(new StringEnumConverter());
                    //options.SerializerSettings.NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore;
                    //options.SerializerSettings.Formatting = Newtonsoft.Json.Formatting.Indented;
                    options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
                    options.JsonSerializerOptions.IgnoreNullValues = true;
                    options.JsonSerializerOptions.PropertyNameCaseInsensitive = true;
                    options.JsonSerializerOptions.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;    // null = default property (Pascal) casing
                    options.JsonSerializerOptions.AllowTrailingCommas = true;
                    //options.JsonSerializerOptions.Converters.Add(new JsonStringTrimConverter());
                })
                //.SetCompatibilityVersion(CompatibilityVersion.Version_3_0)
                //.AddSessionStateTempDataProvider()
                //.AddViewLocalization(Microsoft.AspNetCore.Mvc.Razor.LanguageViewLocationExpanderFormat.Suffix)
                .AddDataAnnotationsLocalization();
            //.AddRazorRuntimeCompilation();

            #endregion AddControllers

            //services.Configure<ApiBehaviorOptions>(options =>
            //{
            //    options.InvalidModelStateResponseFactory = actionContext =>
            //    {
            //        return new ValidationFailedResult(actionContext.ModelState);
            //        //var errors = actionContext.ModelState
            //        //    .Where(e => e.Value.Errors.Count > 0)
            //        //    .Select(e => new Error
            //        //    {
            //        //        Name = e.Key,
            //        //        Message = e.Value.Errors.First().ErrorMessage
            //        //    }).ToArray();

            //        //return new BadRequestObjectResult(errors);
            //    };
            //});

            #region Swagger

            // Register the Swagger generator, defining 1 or more Swagger documents
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo
                {
                    Title = "Fanda API",
                    Version = "v1",
                    Description = "Communicate to Fanda backend from third party background services",
                    TermsOfService = new Uri("https://fanda.in/terms"),
                    Contact = new OpenApiContact
                    {
                        Name = "Balamurugan Thanikachalam",
                        Email = "software.balu@gmail.com",
                        Url = new Uri("https://twitter.com/tbalakpm"),
                    },
                    License = new OpenApiLicense
                    {
                        Name = "Use under MIT",
                        Url = new Uri("https://fanda.in/license"),
                    }
                });

                // Set the comments path for the Swagger JSON and UI.
                var xmlFile = $"{Assembly.GetEntryAssembly().GetName().Name}.xml";
                var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
                c.IncludeXmlComments(xmlPath);

                c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Name = "Authorization",
                    Type = SecuritySchemeType.ApiKey,
                    Scheme = "Bearer",
                    BearerFormat = "JWT",
                    In = ParameterLocation.Header,
                    Description = "JWT Authorization header using the Bearer scheme."
                });

                c.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                          new OpenApiSecurityScheme
                            {
                                Reference = new OpenApiReference
                                {
                                    Type = ReferenceType.SecurityScheme,
                                    Id = "Bearer"
                                }
                            },
                            new string[] {}
                    }
                });
            });

            #endregion Swagger
        }

        public static void ConfigureStartup(this IApplicationBuilder app, IWebHostEnvironment env,
            AutoMapper.IConfigurationProvider autoMapperConfigProvider)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            // else
            // {
            //     app.UseExceptionHandler("/Error");
            //     // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
            //     app.UseHsts();
            // }

            autoMapperConfigProvider.AssertConfigurationIsValid();
            //app.UseHttpsRedirection();
            //app.UseSerilogRequestLogging();

            #region Angular SPA

            //app.UseStaticFiles();
            //if (!env.IsDevelopment())
            //{
            //    app.UseSpaStaticFiles();
            //}

            #endregion Angular SPA

            app.UseRouting();
            //app.UseResponseCaching();
            app.UseResponseCompression();
            // global cors policy
            app.UseCors("_MyAllowedOrigins");
            //app.UseCors(x => x
            //    .SetIsOriginAllowed(origin => true)
            //    .AllowAnyMethod()
            //    .AllowAnyHeader()
            //    .AllowCredentials());
            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                //endpoints.MapControllerRoute(
                //    name: "default",
                //    pattern: "{controller}/{action=Index}/{id?}");
                endpoints.MapControllers()
                    .RequireCors("_MyAllowedOrigins");
                endpoints.MapHealthChecks("/health")
                    .RequireCors("_MyAllowedOrigins");
            });

            #region Angular SPA

            //app.UseSpa(spa =>
            //{
            //    // To learn more about options for serving an Angular SPA from ASP.NET Core,
            //    // see https://go.microsoft.com/fwlink/?linkid=864501

            //    spa.Options.SourcePath = "ClientApp";

            //    if (env.IsDevelopment())
            //    {
            //        spa.UseAngularCliServer(npmScript: "start");
            //    }
            //});

            #endregion Angular SPA

            #region Swagger

            // Enable middleware to serve generated Swagger as a JSON endpoint.
            app.UseSwagger();

            // Enable middleware to serve swagger-ui (HTML, JS, CSS, etc.),
            // specifying the Swagger JSON endpoint.
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "Fanda API V1");
                //c.RoutePrefix = string.Empty;
            });

            #endregion Swagger
        }
    }
}
