namespace TicketingService.Monitoring
{
    using System;
    using System.Linq;
    using System.Threading.Tasks;
    using Abstractions;
    using Be.Vlaanderen.Basisregisters.GrAr.Notifications;
    using Marten;
    using Microsoft.Extensions.Options;

    public class TicketsNotifier
    {
        private readonly IDocumentStore _store;
        private readonly INotificationService _notificationService;
        private readonly NotificationsOptions _options;

        public TicketsNotifier(
            IDocumentStore store,
            INotificationService notificationService,
            IOptions<NotificationsOptions> notificationsOptions)
        {
            _store = store;
            _notificationService = notificationService;
            _options = notificationsOptions.Value;
        }

        public async Task OnTicketsOpenLongerThan(TimeSpan timeWindow)
        {
            await using var session = _store.QuerySession();
            var from = DateTimeOffset.UtcNow.Subtract(timeWindow * 2);
            var until = DateTimeOffset.UtcNow.Subtract(timeWindow);

            var numberOfTickets = new TicketQueryBuilder(session.DocumentStore)
                .FromTo(Operators.WHERE, from, until)
                .ByStatuses(Operators.AND, TicketStatus.Created, TicketStatus.Pending)
                .WhitelistedRegistries(Operators.AND, _options.WhitelistedRegistries)
                .BlacklistedActions(Operators.AND, _options.BlacklistedActions)
                .Execute()
                .Count;

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
