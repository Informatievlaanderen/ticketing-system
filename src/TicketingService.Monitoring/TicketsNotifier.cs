namespace TicketingService.Monitoring
{
    using System;
    using System.Threading.Tasks;
    using Abstractions;
    using Be.Vlaanderen.Basisregisters.GrAr.Notifications;
    using Marten;
    using Microsoft.Extensions.Logging;
    using Microsoft.Extensions.Options;

    public class TicketsNotifier
    {
        private readonly IDocumentStore _store;
        private readonly INotificationService _notificationService;
        private readonly NotificationsOptions _options;
        private readonly ILogger _logger;

        public TicketsNotifier(
            IDocumentStore store,
            INotificationService notificationService,
            IOptions<NotificationsOptions> notificationsOptions,
            ILoggerFactory loggerFactory)
        {
            _store = store;
            _notificationService = notificationService;
            _options = notificationsOptions.Value;
            _logger = loggerFactory.CreateLogger<TicketsNotifier>();
        }

        public async Task OnTicketsOpenLongerThan(TimeSpan timeWindow)
        {
            var from = DateTimeOffset.UtcNow.Subtract(timeWindow * 2);
            var until = DateTimeOffset.UtcNow.Subtract(timeWindow);

            _logger.LogInformation($"Checking for stale tickets between {from} - {until}");

            var numberOfTickets = new TicketQueryBuilder(_store)
                .FromTo(Operators.WHERE, from, until)
                .ByStatuses(Operators.AND, TicketStatus.Created, TicketStatus.Pending)
                .WhitelistedRegistries(Operators.AND, _options.WhitelistedRegistries)
                .BlacklistedActions(Operators.AND, _options.BlacklistedActions)
                .Execute().GetAwaiter().GetResult()
                .Count;

            _logger.LogInformation($"Number of stale tickets: {numberOfTickets}");

            if (numberOfTickets > 0)
            {
                await _notificationService
                    .PublishToTopicAsync(new NotificationMessage(
                        "OpenTickets",
                        $"{numberOfTickets} tickets not completed after {timeWindow:g}.",
                        "Ticketing Monitor",
                        NotificationSeverity.Danger));
            }
        }
    }
}
