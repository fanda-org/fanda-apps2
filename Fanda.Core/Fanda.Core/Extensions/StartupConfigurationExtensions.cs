using System;
using System.IO;
using System.Net.Mime;
using System.Reflection;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;

namespace Fanda.Core.Extensions
{
    public static class StartupConfigurationExtensions
    {
        #region Health checks

        public static IHealthChecksBuilder AddCustomHealthChecks<TDbContext>(this IServiceCollection services)
            where TDbContext : DbContext
        {
            return services
                .AddHealthChecks()
                .AddDbContextCheck<TDbContext>();
        }

        #endregion Health checks

        #region AppSettings

        public static AppSettings ConfigureAppSettings(this IServiceCollection services,
            IConfiguration configuration)
        {
            services.Configure<AppSettings>(configuration);
            var appSettings = configuration.Get<AppSettings>();
            return appSettings;
        }

        #endregion AppSettings

        #region DbContext

        public static IServiceCollection AddCustomDbContext<TDbContext>(this IServiceCollection services,
            AppSettings appSettings, string migrationAssemblyName, bool isDevelopmentEnvironment)
            where TDbContext : DbContext
        {
            return services.AddFandaDbContextPool<TDbContext>(appSettings, migrationAssemblyName,
                isDevelopmentEnvironment);
        }

        #endregion DbContext

        #region CORS

        public static IServiceCollection AddCustomCors(this IServiceCollection services,
            string policyName = "_MyAllowedOrigins")
        {
            return services.AddCors(options =>
            {
                var urls = new[]
                {
                    "http://localhost:4200", // Frontend Angular app from nodejs
                    "http://localhost:55000", // Frontend Angular app from http
                    "http://localhost:5200", // Accounting Service from http
                    "http://localhost:5100", // Authentication Service from http
                    "http://localhost:5000", // API Gateway from http
                    "http://localhost:8000" // nginx proxy server
                    // Configuration["Fanda.Gateway.Url"],
                    // Configuration["Fanda.Ng.Url"]
                };
                options.AddPolicy(policyName, builder =>
                {
                    builder.WithOrigins(urls)
                        .AllowAnyHeader()
                        .AllowAnyMethod()
                        .AllowCredentials();
                });
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
        }

        #endregion CORS

        #region JWT Authentication

        public static AuthenticationBuilder AddJwtAuthentication(this IServiceCollection services,
            string secret)
        {
            var key = Encoding.ASCII.GetBytes(secret);
            return services.AddAuthentication(x =>
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
        }

        #endregion JWT Authentication

        #region AddControllers

        public static IMvcBuilder AddCustomControllers(this IServiceCollection services)
        {
            return services.AddControllers()
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
                    //options.JsonSerializerOptions.DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingDefault;
                    options.JsonSerializerOptions.PropertyNameCaseInsensitive = true;
                    options.JsonSerializerOptions.PropertyNamingPolicy =
                        JsonNamingPolicy.CamelCase; // null = default property (Pascal) casing
                    options.JsonSerializerOptions.AllowTrailingCommas = true;
                    //options.JsonSerializerOptions.Converters.Add(new JsonStringTrimConverter());
                })
                //.SetCompatibilityVersion(CompatibilityVersion.Version_3_0)
                //.AddSessionStateTempDataProvider()
                //.AddViewLocalization(Microsoft.AspNetCore.Mvc.Razor.LanguageViewLocationExpanderFormat.Suffix)
                .AddDataAnnotationsLocalization();
            //.AddRazorRuntimeCompilation();
        }

        #endregion AddControllers

        #region Swagger

        public static IServiceCollection AddSwagger(this IServiceCollection services,
            string apiTitle, string apiDescription = "Fanda API services", string version = "v1")
        {
            // Register the Swagger generator, defining 1 or more Swagger documents
            return services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc(version, new OpenApiInfo
                {
                    Title = apiTitle,
                    Version = version,
                    Description = apiDescription, //"Communicate to Fanda backend from third party background services",
                    TermsOfService = new Uri("https://fandatech.net/terms"),
                    Contact = new OpenApiContact
                    {
                        Name = "Balamurugan Thanikachalam",
                        Email = "software.balu@gmail.com",
                        Url = new Uri("https://twitter.com/tbalakpm")
                    },
                    License = new OpenApiLicense
                    {
                        Name = "Use under MIT", Url = new Uri("https://fandatech.net/license")
                    }
                });

                // Set the comments path for the Swagger JSON and UI.
                string xmlFile = $"{Assembly.GetEntryAssembly().GetName().Name}.xml";
                string xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
                if (File.Exists(xmlPath))
                {
                    c.IncludeXmlComments(xmlPath);
                }

                c.AddSecurityDefinition("Bearer",
                    new OpenApiSecurityScheme
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
                            Reference = new OpenApiReference {Type = ReferenceType.SecurityScheme, Id = "Bearer"}
                        },
                        Array.Empty<string>()
                    }
                });
            });
        }

        #endregion Swagger

        #region Commented - Response Caching

        //services.AddResponseCaching();

        #endregion Commented - Response Caching

        #region Response compression

        //public static IServiceCollection AddResponseCompression(this IServiceCollection services)
        //{
        //    return services.AddResponseCompression(options =>
        //    {
        //        options.Providers.Add<GzipCompressionProvider>();
        //        options.MimeTypes = ResponseCompressionDefaults.MimeTypes.Concat(new[] { "image/svg+xml" });
        //    })
        //        .Configure<GzipCompressionProviderOptions>(options =>
        //        {
        //            options.Level = System.IO.Compression.CompressionLevel.Fastest;
        //        });
        //}

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

        //public static IDataProtectionBuilder AddDataProtection(this IServiceCollection services)
        //{
        //    return services.AddDataProtection()
        //    .UseCryptographicAlgorithms(new AuthenticatedEncryptorConfiguration()
        //    {
        //        EncryptionAlgorithm = EncryptionAlgorithm.AES_256_CBC,
        //        ValidationAlgorithm = ValidationAlgorithm.HMACSHA256
        //    });
        //}

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

        //#region AutoMapper
        //services.AddAutoMapper(typeof(Fanda.Infrastructure.AutoMapperProfiles.AutoMapperProfile));
        //#endregion

        #region Commented - Authorization

        //services.AddAuthorization();

        #endregion Commented - Authorization
    }
}