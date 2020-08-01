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
            AppSettings settings, string migrationsAssemblyName)
            where TDbContext : DbContext
        {
            switch (settings.DatabaseType)
            {
                case "MYSQL":
                    return services.AddEntityFrameworkMySql()
                        .AddDbContextPool<TDbContext>((sp, options) =>
                        {
                            MySqlOptions(sp, options, settings.ConnectionStrings.MySqlConnection,
                                migrationsAssemblyName);
                        })
                        .AddTransient<IDbClient>(_ => new MySqlClient(settings.ConnectionStrings.MySqlConnection));
                case "MARIADB":
                    return services.AddEntityFrameworkMySql()
                        .AddDbContextPool<TDbContext>((sp, options) =>
                        {
                            MariaDbOptions(sp, options, settings.ConnectionStrings.MariaDbConnection,
                                migrationsAssemblyName);
                        })
                        .AddTransient<IDbClient>(_ => new MariaDbClient(settings.ConnectionStrings.MariaDbConnection));
                case "PGSQL":
                    return services.AddEntityFrameworkNpgsql()
                        .AddDbContextPool<TDbContext>((sp, options) =>
                        {
                            PgSqlOptions(sp, options, settings.ConnectionStrings.PgSqlConnection,
                                migrationsAssemblyName);
                        })
                        .AddTransient<IDbClient>(_ => new PgSqlClient(settings.ConnectionStrings.PgSqlConnection));
                case "MSSQL":
                    return services.AddEntityFrameworkSqlServer()
                        .AddDbContextPool<TDbContext>((sp, options) =>
                        {
                            MsSqlOptions(sp, options, settings.ConnectionStrings.MsSqlConnection,
                                migrationsAssemblyName);
                        })
                        .AddTransient<IDbClient>(_ => new MsSqlClient(settings.ConnectionStrings.MsSqlConnection));
                case "SQLITE":
                    return services.AddEntityFrameworkSqlite()
                        .AddDbContextPool<TDbContext>((sp, options) =>
                        {
                            SqliteOptions(sp, options, settings.ConnectionStrings.SqliteConnection,
                                migrationsAssemblyName);
                        })
                        .AddTransient<IDbClient>(_ => new SqliteClient(settings.ConnectionStrings.SqliteConnection));
                case "SQLLOCALDB":
                    return services.AddEntityFrameworkSqlServer()
                        .AddDbContextPool<TDbContext>((sp, options) =>
                        {
                            MsSqlOptions(sp, options, settings.ConnectionStrings.SqlLocalDbConnection,
                                migrationsAssemblyName);
                        })
                        .AddTransient<IDbClient>(_ => new MsSqlClient(settings.ConnectionStrings.SqlLocalDbConnection));
                default:
                    return services.AddEntityFrameworkMySql()
                        .AddDbContextPool<TDbContext>((sp, options) =>
                        {
                            MySqlOptions(sp, options, settings.ConnectionStrings.DefaultConnection,
                                migrationsAssemblyName);
                        })
                        .AddTransient<IDbClient>(_ => new MySqlClient(settings.ConnectionStrings.DefaultConnection));
            }

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
        }

        public static DbContextOptionsBuilder CreateDbContextOptionsBuilder<TDbContext>(AppSettings settings,
            string migrationsAssemblyName)
            where TDbContext : DbContext
        {
            var options = new DbContextOptionsBuilder<TDbContext>();
            switch (settings.DatabaseType)
            {
                case "MYSQL":
                    MySqlOptions(null, options, settings.ConnectionStrings.MySqlConnection,
                        migrationsAssemblyName);
                    break;

                case "MARIADB":
                    MariaDbOptions(null, options, settings.ConnectionStrings.MariaDbConnection,
                        migrationsAssemblyName);
                    break;

                case "PGSQL":
                    PgSqlOptions(null, options, settings.ConnectionStrings.PgSqlConnection,
                        migrationsAssemblyName);
                    break;

                case "MSSQL":
                    MsSqlOptions(null, options, settings.ConnectionStrings.MsSqlConnection,
                        migrationsAssemblyName);
                    break;

                case "SQLITE":
                    SqliteOptions(null, options, settings.ConnectionStrings.SqliteConnection,
                        migrationsAssemblyName);
                    break;

                case "SQLLOCALDB":
                    MsSqlOptions(null, options, settings.ConnectionStrings.SqlLocalDbConnection,
                        migrationsAssemblyName);
                    break;

                default:
                    MySqlOptions(null, options, settings.ConnectionStrings.DefaultConnection,
                        migrationsAssemblyName);
                    break;
            }
            return options;
        }

        private static void MySqlOptions(IServiceProvider sp, DbContextOptionsBuilder options,
            string connectionString, string migrationsAssemblyName)
        {
            options.UseMySql(connectionString, mysqlOptions =>
            {
                mysqlOptions.MigrationsAssembly(migrationsAssemblyName);
                mysqlOptions.ServerVersion(new ServerVersion(new Version(8, 0), ServerType.MySql));
            })
            .UseInternalServiceProvider(sp);
            options.UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking);
            options.EnableDetailedErrors();
            options.EnableSensitiveDataLogging();
            options.EnableServiceProviderCaching();
        }

        private static void MariaDbOptions(IServiceProvider sp, DbContextOptionsBuilder options,
            string connectionString, string migrationsAssemblyName)
        {
            options.UseMySql(connectionString, mysqlOptions =>
            {
                mysqlOptions.MigrationsAssembly(migrationsAssemblyName);
                mysqlOptions.ServerVersion(new ServerVersion(new Version(10, 5), ServerType.MariaDb));
            })
            .UseInternalServiceProvider(sp);
            options.UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking);
            options.EnableDetailedErrors();
            options.EnableSensitiveDataLogging();
            options.EnableServiceProviderCaching();
        }

        private static void PgSqlOptions(IServiceProvider sp, DbContextOptionsBuilder options,
            string connectionString, string migrationsAssemblyName)
        {
            options.UseNpgsql(connectionString, pgsqlOptions =>
            {
                pgsqlOptions.MigrationsAssembly(migrationsAssemblyName);
                pgsqlOptions.EnableRetryOnFailure();
            })
            .UseInternalServiceProvider(sp);
            options.UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking);
            options.EnableDetailedErrors();
            options.EnableSensitiveDataLogging();
            options.EnableServiceProviderCaching();
        }

        private static void MsSqlOptions(IServiceProvider sp, DbContextOptionsBuilder options,
            string connectionString, string migrationsAssemblyName)
        {
            options.UseSqlServer(connectionString, sqlOptions =>
            {
                //sqlOptions.EnableRetryOnFailure();
                //sqlopt.UseRowNumberForPaging();
                sqlOptions.MigrationsAssembly(migrationsAssemblyName);
            })
            .UseInternalServiceProvider(sp);
            options.UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking);
            options.EnableDetailedErrors(true);
            options.EnableSensitiveDataLogging(true);
            options.EnableServiceProviderCaching();
        }

        private static void SqliteOptions(IServiceProvider sp, DbContextOptionsBuilder options,
            string connectionString, string migrationsAssemblyName)
        {
            options.UseSqlite(connectionString, sqlOptions =>
            {
                //sqlOptions.EnableRetryOnFailure();
                //sqlopt.UseRowNumberForPaging();
                sqlOptions.MigrationsAssembly(migrationsAssemblyName);
            })
            .UseInternalServiceProvider(sp);
            options.UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking);
            options.EnableDetailedErrors(true);
            options.EnableSensitiveDataLogging(true);
            options.EnableServiceProviderCaching();
        }
    }
}
