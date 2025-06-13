namespace TicketingService.Monitoring.HealthChecks;

using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.Extensions.Hosting;

public sealed class ServiceHealthCheck<T> : IHealthCheck where T : IHostedService, IHealthCheckable
{
    private readonly T _service;
    private readonly string _name;

    public ServiceHealthCheck(T service, string name)
    {
        _service = service;
        _name = name;
    }

    public Task<HealthCheckResult> CheckHealthAsync(
        HealthCheckContext context,
        CancellationToken cancellationToken = default)
    {
        return Task.FromResult(_service.IsHealthy
            ? HealthCheckResult.Healthy($"{_name} is running normally")
            : HealthCheckResult.Unhealthy($"{_name} is not running or has failed"));
    }
}
