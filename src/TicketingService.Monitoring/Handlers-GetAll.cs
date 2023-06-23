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
        int? offset,
        int? limit,
        CancellationToken cancellationToken)
    {
        const int maxLimit = 500;

        await using var session = store.QuerySession();
        var tickets = await session
            .Query<Ticket>()
            .TicketsFromTo(fromDate, toDate)
            .TicketsPaged(offset is null or < 0 ? 0 : offset.Value, limit is null or > maxLimit ? maxLimit : limit.Value)
            .TicketsByStatuses(ConvertStatuses(statuses))
            .OrderBy(nameof(Ticket.Created))
            .ToListAsync(token: cancellationToken);

        return Results.Json(tickets);
    }
}
