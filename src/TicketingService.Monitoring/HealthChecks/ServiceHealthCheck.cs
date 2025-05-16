namespace TicketingService.Monitoring.HealthChecks;

using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.Extensions.Hosting;

public sealed class ServiceHealthCheck : IHealthCheck
{
    private readonly IHostedService _service;
    private readonly string _name;

    public ServiceHealthCheck(IHostedService service, string name)
    {
        _service = service;
        _name = name;
    }

    public Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context, CancellationToken cancellationToken = default)
    {
        // Check if the service is your custom background service with health status
        if (_service is IHealthCheckable healthCheckable)
        {
            return Task.FromResult(healthCheckable.IsHealthy
                ? HealthCheckResult.Healthy($"{_name} is running normally")
                : HealthCheckResult.Unhealthy($"{_name} is not running or has failed"));
        }

        // Default case if your service doesn't implement the health interface
        return Task.FromResult(HealthCheckResult.Healthy($"{_name} health status unknown"));
    }
}
