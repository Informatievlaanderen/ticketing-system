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

            var tickets = session
                .Query<Ticket>()
                .Where(t =>
                    t.Created >= from
                    && t.Created < until
                    && !t.Status.In(TicketStatus.Complete, TicketStatus.Error))
                .ToList();

            var numberOfTickets = tickets
                .Count(t =>
                    t.Metadata.TryGetValue(MetaDataConstants.Registry, out var registry)
                    && _options.WhitelistedRegistries.Contains(registry)
                    && (!t.Metadata.TryGetValue(MetaDataConstants.Action, out var action)
                        || !_options.BlacklistedActions.Contains(action))
                );

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
