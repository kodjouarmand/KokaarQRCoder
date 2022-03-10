using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;
using KokaarQrCoder.Domain.Contexts;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity.UI.Services;
using KokaarQrCoder.Utility.Options;
using KokaarQrCoder.Infrastructure.Contracts;
using KokaarQrCoder.Infrastructure;
using KokaarQrCoder.Utility.Enum;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Options;
using KokaarQrCoder.Utility.Options.Validations;
using KokaarQrCoder.API.Initializer;

namespace KokaarQrCoder.API.Extensions
{
    public static class ApiServiceExtensions
    {
        public static void ConfigureDbInitializer(this IServiceCollection services)
        {
            services.AddScoped<IDbInitializer, DbInitializer>();
        }

        public static void ConfigureOptions(this IServiceCollection services, IConfiguration configuration)
        {
            services.Configure<LoggingOptions>(configuration.GetSection(LoggingOptions.ConfigSectionName));

            //services.Configure<DbOptions>(configuration.GetSection(DbOptions.ConfigSectionName));
            //services.TryAddSingleton<IValidateOptions<DbOptions>, DbOptionsValidation>();

            services.Configure<SuperAministratorOptions>(configuration.GetSection(SuperAministratorOptions.ConfigSectionName));
            services.TryAddSingleton<IValidateOptions<SuperAministratorOptions>, SuperAministratorOptionsValidation>();
        }

        public static void ConfigureDbContexts(this IServiceCollection services, IConfiguration configuration)
        {
            var dbOptions = new DbOptions();
            configuration.Bind(DbOptions.ConfigSectionName, dbOptions);

            if (dbOptions.ServerType == DBServerTypeEnum.Sqlite.ToString())
            {
                services.AddDbContext<ApplicationDbContext>(opts =>
                   opts.UseSqlite(configuration.GetConnectionString(dbOptions.SqliteConnectionStringName)));
            }
            else if (dbOptions.ServerType == DBServerTypeEnum.SqlServer.ToString())
            {
                services.AddDbContext<ApplicationDbContext>(opts =>
                opts.UseSqlServer(configuration.GetConnectionString(dbOptions.SqlServerConnectionStringName)));
            }
        }

        public static void ConfigureInfrastructureServices(this IServiceCollection services)
        {
            services.AddScoped<ILoggerService, LoggerService>();
        }

        public static void ConfigureControllers(this IServiceCollection services)
        {
            services.AddControllers();
        }

        public static void ConfigureSwagger(this IServiceCollection services)
        {
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "KokaarQrCoder.API", Version = "v1" });
            });
        }
    }
}
