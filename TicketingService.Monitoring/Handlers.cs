namespace TicketingService.Monitoring;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Abstractions;
using Marten;

public static partial class Handlers
{
    public static DateTimeOffset DefaultFromDateFilter => DateTime.Now.AddMonths(-1);

    private static IQueryable<Ticket> TicketsFromTo(
        this IQuerySession session,
        string? fromDate,
        string? toDate)
    {
        var (from, to) = CreateDateRange(fromDate, toDate);

        return session.Query<Ticket>()
            .Where(t =>
                t.Created >= from.UtcDateTime
                && t.Created <= to.UtcDateTime);
    }

    private static async Task<IEnumerable<Ticket>> TicketsByStatuses(
        this IQueryable<Ticket> tickets,
        TicketStatus[] statuses,
        CancellationToken cancellationToken)
    {
        var resolvedResult = await tickets.ToListAsync(cancellationToken);

        return resolvedResult.Where(t => statuses.Contains(t.Status));
    }

    private static IQueryable<Ticket> WithStatus(this IQueryable<Ticket> query, TicketStatus status)
    {
        return query.Where(t => t.Status == status);
    }

    private static async Task<IEnumerable<Ticket>> TicketsByAction(
        this IQueryable<Ticket> tickets,
        string registry,
        string action)
    {
        var resolvedResult = await tickets.ToListAsync();

        // In-memory filtering
        return resolvedResult.Where(t =>
            string.Equals(t.Metadata.First(m => m.Key == MetaDataConstants.Registry).Value, registry, StringComparison.InvariantCultureIgnoreCase)
            && string.Equals(t.Metadata.First(m => m.Key == MetaDataConstants.Action).Value, action, StringComparison.InvariantCultureIgnoreCase));
    }

    private static DateTimeOffset GetStartDate(string? fromDate)
    {
        return DateTimeOffset.TryParse(fromDate, out var dateTime)
            ? dateTime
            : DefaultFromDateFilter;
    }

    private static DateTimeOffset GetEndDate(DateTimeOffset fromDate, string? toDate)
    {
        if (DateTimeOffset.TryParse(toDate, out var dateTime) && dateTime > fromDate)
        {
            return dateTime;
        }

        return DateTimeOffset.Now.Date;
    }

    private static (DateTimeOffset from, DateTimeOffset to) CreateDateRange(string? fromDate, string? toDate)
    {
        var from = GetStartDate(fromDate);
        var to = GetEndDate(from, toDate);
        return (from, to);
    }

    private static TicketStatus[]? ConvertStatuses(string? statuses)
    {
        if (string.IsNullOrEmpty(statuses))
        {
            return null;
        }

        var statusList = statuses
            .Replace(" ", string.Empty)
            .Split(",")
            .ToList();

        var statusesToFilterOn = new List<TicketStatus>();
        statusList.ForEach(status =>
        {
            if (Enum.TryParse(status, true, out TicketStatus ts))
            {
                statusesToFilterOn.Add(ts);
            }
            else
            {
                throw new InvalidOperationException($"Status '{status}' could not be parsed.");
            }
        });

        return statusesToFilterOn.ToArray();
    }

    private static string CreateActionString(Ticket ticket)
        => $"{ticket.Metadata.GetValue(MetaDataConstants.Registry)}.{ticket.Metadata.GetValue(MetaDataConstants.Action)}";

    public static string GetValue(this IDictionary<string, string> dict, string key)
    {
        return dict.TryGetValue(key, out var value) ? value : string.Empty;
    }
}
