namespace TicketingService.Monitoring.HealthChecks;

public interface IHealthCheckable
{
    bool IsHealthy { get; }
}
