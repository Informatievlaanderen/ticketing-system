using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using TicketingService.Monitoring;

var app = WebApplication
    .CreateBuilder(args)
    .AddAppSettings(args)
    .AddOptions<ConnectionStrings>()
    .AddOptions<NotificationsOptions>()
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

app.MapGet("/all", Handlers.GetAll);
app.MapGet("/distribution", Handlers.GetDistribution);
app.MapGet("/distribution/action", Handlers.GetDistributionAction);

app.UseSwagger();
app.UseSwaggerUI();

app.UseHttpLogging();

app.Run();
