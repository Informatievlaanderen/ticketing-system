namespace TicketingService.Monitoring;

using System;
using System.Threading;
using System.Threading.Tasks;
using Abstractions;
using Be.Vlaanderen.Basisregisters.GrAr.Notifications;
using HealthChecks;
using Marten;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

public sealed class ExpiredTicketsRemover : IHostedService, IDisposable, IHealthCheckable
{
    private static readonly TimeSpan Interval = TimeSpan.FromDays(7);

    private Timer? _timer;

    private readonly IDocumentStore _store;
    private readonly ILogger<ExpiredTicketsRemover> _logger;
    private readonly INotificationService _notificationService;

    public bool IsHealthy { get; private set; } = true;

    public ExpiredTicketsRemover(
        IDocumentStore store,
        INotificationService notificationService,
        ILoggerFactory loggerFactory)
    {
        _store = store;
        _notificationService = notificationService;
        _logger = loggerFactory.CreateLogger<ExpiredTicketsRemover>();
    }


    public Task StartAsync(CancellationToken cancellationToken)
    {
        _logger.LogInformation($"Starting expired tickets background service, monitoring every {Interval:g}.");

        _timer = new Timer(DoWork, null, TimeSpan.Zero, Interval);

        return Task.CompletedTask;
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        _logger.LogInformation("Stopping expired tickets background service.");
        _timer?.Change(Timeout.Infinite, 0);

        return Task.CompletedTask;
    }

    private void DoWork(object? state)
    {
        _logger.LogInformation($"Checking for expired tickets.");

        try
        {
            using var session = _store.LightweightSession();
            session.DeleteWhere<Ticket>(t => t.LastModified < DateTime.UtcNow.AddYears(-1));

            session.SaveChangesAsync();
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Error while deleting expired tickets");
            _notificationService.PublishToTopicAsync(
                new NotificationMessage(
                    "TicketingService",
                    "Error while deleting expired tickets" + Environment.NewLine + e,
                    "Ticketing Service",
                    NotificationSeverity.Danger)
                ).Wait();
            IsHealthy = false;
        }
    }

    public void Dispose()
    {
        _timer?.Dispose();
    }
}
