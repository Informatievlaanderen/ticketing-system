namespace TicketingService.Monitoring;

using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Marten;
using Microsoft.AspNetCore.Http;

public static partial class Handlers
{
    public static async Task<IResult> GetList(
        IDocumentStore store,
        string? fromDate,
        string? toDate,
        string? statuses,
        CancellationToken cancellationToken)
    {
        await using var session = store.QuerySession();
        var tickets = await session
            .TicketsFromTo(fromDate, toDate)
            .TicketsByStatuses(
                ConvertStatuses(statuses),
                cancellationToken);

        var result = tickets
            .Select(ticket => new
            {
                TimeSinceCreation = DateTimeOffset.Now.Subtract(ticket.Created),
                Ticket = ticket
            })
            .OrderByDescending(t => t.TimeSinceCreation);

        return Results.Json(result);
    }
}
