namespace TicketingService.Monitoring;

using System.Linq;
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
        string? registries,
        int? offset,
        int? limit,
        CancellationToken cancellationToken)
    {
        const int maxLimit = 500;

        await using var session = store.QuerySession();

        var tickets = new TicketQueryBuilder(session.DocumentStore)
            .FromTo(Operators.WHERE, fromDate, toDate)
            .WhitelistedRegistries(Operators.AND, SplitRegistryString(registries))
            .ByStatuses(Operators.AND, ConvertStatuses(statuses))
            .Paged(offset is null or < 0 ? 0 : offset.Value, limit is null or > maxLimit ? maxLimit : limit.Value)
            .Execute().GetAwaiter().GetResult()
            .AsQueryable()
            .OrderBy(nameof(Ticket.Created));

        return Results.Json(tickets);
    }
}
