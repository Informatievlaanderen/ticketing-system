using System.ComponentModel.DataAnnotations;
using System.Reflection;
using TicketingService.Storage.PgSqlMarten;

namespace TicketingService.Monitoring;

public static class StartupExtensions
{
    public static WebApplicationBuilder AddAppSettings(this WebApplicationBuilder builder, string[] args)
    {
        builder.Configuration
            .AddJsonFile("appsettings.json")
            .AddJsonFile($"appsettings.{builder.Environment.EnvironmentName.ToLowerInvariant()}.json", optional: true, reloadOnChange: false)
            .AddJsonFile($"appsettings.{Environment.MachineName.ToLowerInvariant()}.json", optional: true, reloadOnChange: false)
            .AddEnvironmentVariables()
            .AddCommandLine(args);

        return builder;
    }

    public static WebApplicationBuilder AddServices(this WebApplicationBuilder builder)
    {
        var options = builder.GetAppOptions<AppOptions>();

        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();
        builder.Services
            .AddMartenTicketing(options.ConnectionString)
            .AddHealthChecks().AddNpgSql(_ => options.ConnectionString);

        return builder;
    }

    public static WebApplicationBuilder AddOptions<TOptions>(this WebApplicationBuilder builder)
        where TOptions : class
    {
        builder.Services.Configure<TOptions>(builder.Configuration.GetSection(typeof(TOptions).Name));
        return builder;
    }

    public static TOptions GetAppOptions<TOptions>(this WebApplicationBuilder builder)
        where TOptions : class, new()
    {
        var options = new TOptions();
        builder.Configuration.Bind(typeof(TOptions).Name, options);

        var requiredProperties = options
            .GetType()
            .GetProperties()
            .Where(prop => Attribute.IsDefined(prop, typeof(RequiredAttribute)));

        void ThrowArgumentNullException(PropertyInfo p) => throw new ArgumentNullException($"{typeof(TOptions).Name}.{p.Name}");

        foreach (var prop in requiredProperties)
        {
            var obj = prop.GetValue(options, null);

            if (prop.GetValue(options, null) is null)
            {
                ThrowArgumentNullException(prop);
            }

            if (obj is string valstr && string.IsNullOrEmpty(valstr))
            {
                ThrowArgumentNullException(prop);
            }
        }

        return options;
    }
}
