namespace TicketingService.Monitoring.HealthChecks;

using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.Extensions.Hosting;

public sealed class ServiceHealthCheck<T> : IHealthCheck where T : IHostedService, IHealthCheckable
{
    private readonly T _service;

    public ServiceHealthCheck(T service)
    {
        _service = service;
    }

    public Task<HealthCheckResult> CheckHealthAsync(
        HealthCheckContext context,
        CancellationToken cancellationToken = default)
    {
        return Task.FromResult(_service.IsHealthy
            ? HealthCheckResult.Healthy($"{_service.GetType().Name} is running normally")
            : HealthCheckResult.Unhealthy($"{_service.GetType().Name} is not healthy"));
    }
}
