namespace TicketingService.Monitoring;

using System;
using Abstractions;
using Marten;

public static partial class Handlers
{
    public static async Task<IResult> GetActionDistribution(
        IDocumentStore store,
        string? fromDate,
        string? toDate,
        string registry,
        string action,
        string? aggregateId,
        int? groupCount,
        CancellationToken cancellationToken)
    {
        var groupCount2 = groupCount ?? DefaultDistributionGroupCount;

        await using var session = store.QuerySession();
        var tickets = await session
            .TicketsFromTo(fromDate, toDate)
            .WithStatus(TicketStatus.Complete)
            .TicketsByAction(registry, action);

        if (!string.IsNullOrEmpty(aggregateId))
        {
            tickets = tickets.Where(t =>
                string.Equals(
                    t.Metadata.First(m => m.Key == MetaDataConstants.AggregateId).Value,
                    registry, StringComparison.InvariantCultureIgnoreCase));
        }

        var completionTimes = tickets
            .Select(t => (
                ExecutionTime: t.LastModified.Subtract(t.Created),
                Action: CreateActionString(t),
                AggregateId: t.Metadata.First(m => m.Key == MetaDataConstants.AggregateId).Value))
            .OrderBy(t => t.ExecutionTime)
            .ToList();

        var groupSize = completionTimes.Max(t => t.ExecutionTime).TotalMilliseconds / (groupCount2*1000);

        var ranges = CreateDistributionRanges(groupCount2, groupSize);

        var result = new List<object>();
        foreach (var (min, max) in ranges)
        {
            var totalInRange = completionTimes
                .Where(t => t.ExecutionTime.TotalMilliseconds > min && t.ExecutionTime.TotalMilliseconds < max)
                .ToList();

            var distinctActions = totalInRange
                .Select(t => $"{t.Action} - AggregateId: {t.AggregateId}")
                .OrderBy(t => t).ToList();

            if (totalInRange.Any())
            {
                result.Add(new
                {
                    ExecutionTime = $"{TimeSpan.FromMilliseconds(min)} - {TimeSpan.FromMilliseconds(max)}",
                    totalInRange.Count,
                    Actions = distinctActions
                });
            }
        }

        return Results.Json(result);
    }
}
