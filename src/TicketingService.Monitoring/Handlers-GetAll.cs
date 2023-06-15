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
        int offset,
        int limit,
        string? statuses,
        CancellationToken cancellationToken)
    {
        var max = 500;

        await using var session = store.QuerySession();
        var tickets = await session
            .TicketsFromTo(fromDate, toDate)
            .TicketsPaged(offset, limit > max ? max : limit)
            .TicketsByStatuses(
                ConvertStatuses(statuses) ?? new[] {TicketStatus.Pending, TicketStatus.Created, TicketStatus.Complete, TicketStatus.Error},
                cancellationToken);

        return Results.Json(tickets);
    }
}
