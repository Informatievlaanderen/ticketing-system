namespace TicketingService.Monitoring;

using System.Threading;
using System.Threading.Tasks;
using Abstractions;
using Marten;
using Microsoft.AspNetCore.Http;

public static partial class Handlers
{
    public static async Task<IResult> GetAll(
        IDocumentStore store,
        string? fromDate,
        string? toDate,
        string? statuses,
        int offset,
        int limit,
        CancellationToken cancellationToken)
    {
        var max = 500;

        await using var session = store.QuerySession();
        var tickets = await session
            .Query<Ticket>()
            .TicketsFromTo(fromDate, toDate)
            .TicketsPaged(offset, limit > max ? max : limit)
            .TicketsByStatuses(ConvertStatuses(statuses))
            .OrderBy(nameof(Ticket.Created))
            .ToListAsync(token: cancellationToken);

        return Results.Json(tickets);
    }
}
