using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.Extensions.Diagnostics.HealthChecks;
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
app.MapGet("distribution/action", Handlers.GetActionDistribution);
app.MapGet("/list", Handlers.GetList);

app.UseSwagger();
app.UseSwaggerUI();

app.UseHttpLogging();

app.Run();