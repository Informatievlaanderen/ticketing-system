using Marten;
using Newtonsoft.Json;
using TicketingService.Abstractions;

namespace TicketingService.Monitoring
{
    public static class Handlers
    {
        private static DateTimeOffset _fromDate = new DateTimeOffset(new DateTime(2022, 10, 20), TimeSpan.Zero);

        public static async Task<string> GetDistribution(IDocumentStore store, CancellationToken cancellationToken)
        {
            var completedTickets = await store.GetCompletedTickets(cancellationToken);

            var completionTimes = completedTickets
                .Select(t => t.LastModified.Subtract(t.Created))
                .OrderBy(t => t);

            var groupCount = 10;
            var groupSize = Math.Ceiling(completionTimes.Max().TotalSeconds / groupCount);

            var groups = new List<KeyValuePair<string, int>>();

            var min = 0d;
            var max = groupSize;
            for (int i = 0; i < groupCount; i++)
            {
                var totalInRange = completionTimes.Where(t => t.TotalSeconds > min && t.TotalSeconds <= max);
                groups.Add(new($"{min} - {max}", totalInRange.Count()));
                min += groupSize;
                max += groupSize;
            }

            return JsonConvert.SerializeObject(new
            {
                Distribution = groups
            });
        }

        public static async Task<string> GroupByRegistry(IDocumentStore store, CancellationToken cancellationToken)
        {
            return JsonConvert.SerializeObject("Not implemented");
        }

        private static async Task<IReadOnlyList<Ticket>> GetCompletedTickets(this IDocumentStore store, CancellationToken cancellationToken)
        {
            await using var session = store.QuerySession();
            return await session.Query<Ticket>()
                .Where(t => t.Status == TicketStatus.Complete && t.Created > _fromDate)
                .ToListAsync(cancellationToken);
        }
    }
}
