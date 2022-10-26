using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Newtonsoft.Json;
using TicketingService.Monitoring;

var app = WebApplication
    .CreateBuilder(args)
    .AddAppSettings(args)
    .AddOptions<AppOptions>()
    .AddServices()
    .Build();

app
    .UseHealthChecks(new PathString("/health"), new HealthCheckOptions
    {
        ResultStatusCodes =
        {
            [HealthStatus.Healthy] = StatusCodes.Status200OK,
            [HealthStatus.Degraded] = StatusCodes.Status200OK,
            [HealthStatus.Unhealthy] = StatusCodes.Status503ServiceUnavailable
        }
    });

app.MapGet("/distribution", Handlers.GetDistribution);
app.MapGet("/registry", Handlers.GroupByRegistry);

app.UseSwagger();
app.UseSwaggerUI();

app.Run();

public class AppOptions
{
    [Required]
    public string ConnectionString { get; set; }
}
