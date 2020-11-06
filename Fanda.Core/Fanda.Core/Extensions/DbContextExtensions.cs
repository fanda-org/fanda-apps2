using Fanda.Core.SqlClients;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Pomelo.EntityFrameworkCore.MySql.Infrastructure;
using Pomelo.EntityFrameworkCore.MySql.Storage;
using System;

namespace Fanda.Core.Extensions
{
    public static class DbContextExtensions
    {
        public static IServiceCollection AddFandaDbContextPool<TDbContext>(this IServiceCollection services,
            AppSettings settings, string migrationsAssemblyName, bool isDevelopmentEnvironment)
            where TDbContext : DbContext
        {
            switch (settings.DatabaseType)
            {
                case "MYSQL":
                    return services // .AddEntityFrameworkMySql()
                        .AddDbContextPool<TDbContext>(options => // (sp, options)
                        {
                            MySqlOptions(options, settings.ConnectionStrings.MySqlConnection,
                                migrationsAssemblyName, isDevelopmentEnvironment);
                        })
                        .AddTransient<IDbClient>(_ => new MySqlClient(settings.ConnectionStrings.MySqlConnection));
                case "MARIADB":
                    return services // .AddEntityFrameworkMySql()
                        .AddDbContextPool<TDbContext>(options => // (sp, options)
                        {
                            MariaDbOptions(options, settings.ConnectionStrings.MariaDbConnection,
                                migrationsAssemblyName, isDevelopmentEnvironment);
                        })
                        .AddTransient<IDbClient>(_ => new MariaDbClient(settings.ConnectionStrings.MariaDbConnection));
                case "PGSQL":
                    return services // .AddEntityFrameworkNpgsql()
                        .AddDbContextPool<TDbContext>(options => // (sp, options)
                        {
                            PgSqlOptions(options, settings.ConnectionStrings.PgSqlConnection,
                                migrationsAssemblyName, isDevelopmentEnvironment);
                        })
                        .AddTransient<IDbClient>(_ => new PgSqlClient(settings.ConnectionStrings.PgSqlConnection));
                case "MSSQL":
                    return services // .AddEntityFrameworkSqlServer()
                        .AddDbContextPool<TDbContext>(options => // (sp, options)
                        {
                            MsSqlOptions(options, settings.ConnectionStrings.MsSqlConnection,
                                migrationsAssemblyName, isDevelopmentEnvironment);
                        })
                        .AddTransient<IDbClient>(_ => new MsSqlClient(settings.ConnectionStrings.MsSqlConnection));
                case "SQLITE":
                    return services // .AddEntityFrameworkSqlite()
                        .AddDbContextPool<TDbContext>(options => // (sp, options)
                        {
                            SqliteOptions(options, settings.ConnectionStrings.SqliteConnection,
                                migrationsAssemblyName, isDevelopmentEnvironment);
                        })
                        .AddTransient<IDbClient>(_ => new SqliteClient(settings.ConnectionStrings.SqliteConnection));
                case "SQLLOCALDB":
                    return services // .AddEntityFrameworkSqlServer()
                        .AddDbContextPool<TDbContext>(options => // (sp, options)
                        {
                            MsSqlOptions(options, settings.ConnectionStrings.SqlLocalDbConnection,
                                migrationsAssemblyName, isDevelopmentEnvironment);
                        })
                        .AddTransient<IDbClient>(_ => new MsSqlClient(settings.ConnectionStrings.SqlLocalDbConnection));
                default:
                    return services // .AddEntityFrameworkMySql()
                        .AddDbContextPool<TDbContext>(options => // (sp, options)
                        {
                            MySqlOptions(options, settings.ConnectionStrings.DefaultConnection,
                                migrationsAssemblyName, isDevelopmentEnvironment);
                        })
                        .AddTransient<IDbClient>(_ => new MySqlClient(settings.ConnectionStrings.DefaultConnection));
            }

            #region Commented - Password policy, IdentityServer

            //services.AddIdentity<User, Role>(options =>
            //{
            //    //options.SignIn.RequireConfirmedEmail = true;
            //    // Password settings
            //    options.Password.RequireDigit = true;
            //    options.Password.RequiredLength = 6;
            //    options.Password.RequireNonAlphanumeric = false;
            //    options.Password.RequireUppercase = true;
            //    options.Password.RequireLowercase = false;
            //    options.Password.RequiredUniqueChars = 3;

            //    // Lockout settings
            //    options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(5);
            //    options.Lockout.MaxFailedAccessAttempts = 5;
            //    options.Lockout.AllowedForNewUsers = true;

            //    // User settings
            //    options.User.RequireUniqueEmail = true;
            //})
            //.AddEntityFrameworkStores<FandaContext>()
            //.AddDefaultTokenProviders();

            //services.AddIdentityServer()
            //    .AddDeveloperSigningCredential()
            //    .AddInMemoryPersistedGrants()
            //    .AddInMemoryIdentityResources(IdentityServerConfig.GetIdentityResources())
            //    .AddInMemoryApiResources(IdentityServerConfig.GetApiResources())
            //    .AddInMemoryClients(IdentityServerConfig.GetClients())
            //    .AddAspNetIdentity<User>();

            #endregion Commented - Password policy, IdentityServer
        }

        public static DbContextOptionsBuilder CreateDbContextOptionsBuilder<TDbContext>(AppSettings settings,
            string migrationsAssemblyName, bool isDevelopmentEnvironment)
            where TDbContext : DbContext
        {
            var options = new DbContextOptionsBuilder<TDbContext>();
            switch (settings.DatabaseType)
            {
                case "MYSQL":
                    MySqlOptions(options, settings.ConnectionStrings.MySqlConnection,
                        migrationsAssemblyName, isDevelopmentEnvironment);
                    break;

                case "MARIADB":
                    MariaDbOptions(options, settings.ConnectionStrings.MariaDbConnection,
                        migrationsAssemblyName, isDevelopmentEnvironment);
                    break;

                case "PGSQL":
                    PgSqlOptions(options, settings.ConnectionStrings.PgSqlConnection,
                        migrationsAssemblyName, isDevelopmentEnvironment);
                    break;

                case "MSSQL":
                    MsSqlOptions(options, settings.ConnectionStrings.MsSqlConnection,
                        migrationsAssemblyName, isDevelopmentEnvironment);
                    break;

                case "SQLITE":
                    SqliteOptions(options, settings.ConnectionStrings.SqliteConnection,
                        migrationsAssemblyName, isDevelopmentEnvironment);
                    break;

                case "SQLLOCALDB":
                    MsSqlOptions(options, settings.ConnectionStrings.SqlLocalDbConnection,
                        migrationsAssemblyName, isDevelopmentEnvironment);
                    break;

                default:
                    MySqlOptions(options, settings.ConnectionStrings.DefaultConnection,
                        migrationsAssemblyName, isDevelopmentEnvironment);
                    break;
            }

            return options;
        }

        private static void MySqlOptions( /*IServiceProvider sp,*/ DbContextOptionsBuilder options,
            string connectionString, string migrationsAssemblyName, bool isDevelopmentEnvironment)
        {
            options.UseMySql(connectionString, mysqlOptions =>
                {
                    mysqlOptions.MigrationsAssembly(migrationsAssemblyName);
                    mysqlOptions.ServerVersion(new ServerVersion(new Version(8, 0)));
                })
                // .UseInternalServiceProvider(sp);
                // .UseSnakeCaseNamingConvention()
                // .UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking)
                .EnableDetailedErrors(isDevelopmentEnvironment)
                .EnableSensitiveDataLogging(isDevelopmentEnvironment)
                .EnableServiceProviderCaching();
        }

        private static void MariaDbOptions( /*IServiceProvider sp,*/ DbContextOptionsBuilder options,
            string connectionString, string migrationsAssemblyName, bool isDevelopmentEnvironment)
        {
            options.UseMySql(connectionString, mysqlOptions =>
                {
                    mysqlOptions.MigrationsAssembly(migrationsAssemblyName);
                    mysqlOptions.ServerVersion(new ServerVersion(new Version(10, 5), ServerType.MariaDb));
                })
                // .UseInternalServiceProvider(sp);
                // .UseSnakeCaseNamingConvention()
                // .UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking)
                .EnableDetailedErrors(isDevelopmentEnvironment)
                .EnableSensitiveDataLogging(isDevelopmentEnvironment)
                .EnableServiceProviderCaching();
        }

        private static void PgSqlOptions( /*IServiceProvider sp,*/ DbContextOptionsBuilder options,
            string connectionString, string migrationsAssemblyName, bool isDevelopmentEnvironment)
        {
            options.UseNpgsql(connectionString, pgsqlOptions =>
                {
                    pgsqlOptions.MigrationsAssembly(migrationsAssemblyName);
                    pgsqlOptions.EnableRetryOnFailure();
                })
                // .UseInternalServiceProvider(sp);
                // .UseSnakeCaseNamingConvention()
                // .UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking)
                .EnableDetailedErrors(isDevelopmentEnvironment)
                .EnableSensitiveDataLogging(isDevelopmentEnvironment)
                .EnableServiceProviderCaching();
        }

        private static void MsSqlOptions( /*IServiceProvider sp,*/ DbContextOptionsBuilder options,
            string connectionString, string migrationsAssemblyName, bool isDevelopmentEnvironment)
        {
            options.UseSqlServer(connectionString, sqlOptions =>
                {
                    //sqlOptions.EnableRetryOnFailure();
                    //sqlopt.UseRowNumberForPaging();
                    sqlOptions.MigrationsAssembly(migrationsAssemblyName);
                })
                // .UseInternalServiceProvider(sp);
                // .UseSnakeCaseNamingConvention()
                // .UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking)
                .EnableDetailedErrors(isDevelopmentEnvironment)
                .EnableSensitiveDataLogging(isDevelopmentEnvironment)
                .EnableServiceProviderCaching();
        }

        private static void SqliteOptions( /*IServiceProvider sp,*/ DbContextOptionsBuilder options,
            string connectionString, string migrationsAssemblyName, bool isDevelopmentEnvironment)
        {
            options.UseSqlite(connectionString, sqlOptions =>
                {
                    sqlOptions.MigrationsAssembly(migrationsAssemblyName);
                })
                // .UseInternalServiceProvider(sp);
                // .UseSnakeCaseNamingConvention()
                // .UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking)
                .EnableDetailedErrors(isDevelopmentEnvironment)
                .EnableSensitiveDataLogging(isDevelopmentEnvironment)
                .EnableServiceProviderCaching();
        }
    }
}