namespace TicketingService.Extensions;

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Diagnostics.HealthChecks;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddHealthChecksFromConfiguration(this IServiceCollection services, HealthOptions healthOptions, string connectionString)
    {
        if (healthOptions.PerformPostgresHealthCheck)
        {
            services.AddHealthChecks()
                .AddNpgSql(_ => connectionString);
        }
        else
        {
            services.AddHealthChecks()
                .AddCheck("Health", () => HealthCheckResult.Healthy());
        }
        return services;
    }
}
