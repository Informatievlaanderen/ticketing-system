namespace TicketingService.Monitoring;

using System;
using Abstractions;
using Marten;

public static partial class Handlers
{
    public const int DefaultDistributionGroupCount = 50;

    public static async Task<IResult> GetDistribution(
        IDocumentStore store,
        string? fromDate,
        string? toDate,
        int? groupCount,
        CancellationToken cancellationToken)
    {
        var groupCount2 = groupCount ?? DefaultDistributionGroupCount;

        await using var session = store.QuerySession();
        var tickets = await session
            .TicketsFromTo(fromDate, toDate)
            .WithStatus(TicketStatus.Complete)
            .ToListAsync(cancellationToken);

        var completionTimes = tickets
            .Select(t => (
                ExecutionTime: t.LastModified.Subtract(t.Created),
                Action: CreateActionString(t)))
            .OrderBy(t => t.ExecutionTime);

        var groupSize = completionTimes.Max(t => t.ExecutionTime).TotalMilliseconds / (groupCount2*1000);

        var ranges = CreateDistributionRanges(groupCount2, groupSize);

        var result = new List<object>();
        foreach (var (min, max) in ranges)
        {
            var totalInRange = completionTimes
                .Where(t => t.ExecutionTime.TotalMilliseconds > min && t.ExecutionTime.TotalMilliseconds < max);

            var distinctActions = totalInRange
                .DistinctBy(t => t.Action)
                .Select(t => t.Action)
                .OrderBy(t => t).ToList();

            if (totalInRange.Any())
            {
                result.Add(new
                {
                    ExecutionTime = $"{TimeSpan.FromMilliseconds(min)} - {TimeSpan.FromMilliseconds(max)}",
                    Count = totalInRange.Count(),
                    Actions = distinctActions
                });
            }
        }
        return Results.Json(result);
    }

    private static IEnumerable<(double min, double max)> CreateDistributionRanges(int groupCount, double groupSize)
    {
        return from i in Enumerable.Range(0, groupCount)
            let min = i * groupSize
            let max = (i + 1) * groupSize
            select (min, max);
    }
}