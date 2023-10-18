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

        private DateTimeOffset? _until;

        public async Task OnTicketsOpenLongerThan(TimeSpan timeWindow)
        {
            var from = DateTimeOffset.UtcNow.Subtract(timeWindow * 2);

            if (_until is not null)
            {
                from = _until.Value;
            }

            _until = DateTimeOffset.UtcNow.Subtract(timeWindow);

            _logger.LogInformation($"Checking for stale tickets between {from} - {_until}");

            var numberOfTickets = new TicketQueryBuilder(_store)
                .FromTo(Operators.WHERE, from, _until.Value)
                .ByStatuses(Operators.AND, TicketStatus.Created, TicketStatus.Pending, TicketStatus.Complete)
                .WhitelistedRegistries(Operators.AND, _options.WhitelistedRegistries)
                .BlacklistedActions(Operators.AND, _options.BlacklistedActions)
                .Execute().GetAwaiter().GetResult()
                .Count;

            _logger.LogInformation($"{numberOfTickets} stale tickets between {from} - {_until.Value}");

            if (numberOfTickets > 0)
            {
                _logger.LogWarning($"{numberOfTickets} stale tickets between {from} - {_until.Value}");
                
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
