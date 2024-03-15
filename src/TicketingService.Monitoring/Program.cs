using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpLogging;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using TicketingService.Monitoring;

var appBuilder = WebApplication
    .CreateBuilder(args);
appBuilder.Services.AddHttpLogging(logging =>
{
    logging.LoggingFields = HttpLoggingFields.All;
    logging.RequestBodyLogLimit = 4096;
});
var app = appBuilder
    .AddAppSettings(args)
    .AddOptions<ConnectionStrings>()
    .AddOptions<NotificationsOptions>()
    .AddServices()
    .AddLogging()
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
