namespace TicketingService.Monitoring
{
    using System;
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;
    using Abstractions;
    using Be.Vlaanderen.Basisregisters.GrAr.Notifications;
    using Marten;
    using Microsoft.Extensions.Hosting;
    using Microsoft.Extensions.Logging;
    using Microsoft.Extensions.Options;

    public class NotificationsOptions
    {
        public string[] WhitelistedRegistries { get; set; }
        public string[] BlacklistedActions { get; set; }
    }

    public class NotificationBackgroundService :  IHostedService, IDisposable
    {
        private static readonly TimeSpan PollingInterval = TimeSpan.FromMinutes(5);

        private readonly INotificationService _notificationService;
        private readonly IDocumentStore _store;
        private readonly NotificationsOptions _options;
        private readonly ILogger _logger;

        private Timer? _timer;

        public NotificationBackgroundService(
            ILoggerFactory loggerFactory,
            INotificationService notificationService,
            IDocumentStore store,
            IOptions<NotificationsOptions> options)
        {
            _notificationService = notificationService;
            _store = store;
            _options = options.Value;
            _logger = loggerFactory.CreateLogger<NotificationBackgroundService>();
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation($"Starting notification background service, polling every {PollingInterval:g}.");
            _timer = new Timer(DoWork, null, TimeSpan.Zero, PollingInterval);

            return Task.CompletedTask;
        }

        private void DoWork(object? state)
        {
            using var session = _store.QuerySession();
            var from = DateTimeOffset.Now.Subtract(PollingInterval * 2);
            var until =  DateTimeOffset.Now.Subtract(PollingInterval);

            var tickets = session
                .Query<Ticket>()
                .Where(t => t.Created >= from && t.Created < until)
                .Where(t => t.Status.In(TicketStatus.Created, TicketStatus.Pending))
                .ToList();

            var numberOfTickets = tickets
                .Count(t =>
                    t.Metadata.TryGetValue(MetaDataConstants.Registry, out var registry)
                    && _options.WhitelistedRegistries.Contains(registry)
                    && (!t.Metadata.TryGetValue(MetaDataConstants.Action, out var action)
                        || !_options.BlacklistedActions.Contains(action)));

            if (numberOfTickets > 0)
            {
                _notificationService
                    .PublishToTopicAsync(new NotificationMessage(
                        "OpenTickets",
                        $"{numberOfTickets} tickets not completed after {PollingInterval:g}.",
                        "Ticketing Monitor",
                        NotificationSeverity.Danger))
                    .Wait();
            }
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
           _logger.LogInformation("Stopping NotificationBackgroundService");
           _timer?.Change(Timeout.Infinite, 0);

           return Task.CompletedTask;
        }

        public void Dispose()
        {
            _timer?.Dispose();
        }
    }
}
