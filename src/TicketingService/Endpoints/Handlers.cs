namespace TicketingService.Endpoints;

using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Abstractions;
using Datadog.Trace;
using Extensions;
using Microsoft.AspNetCore.Mvc;

public static class Handlers
{
    public static async Task<Guid> Create([FromBody] IDictionary<string, string>? metadata, ITicketing ticketing,
        CancellationToken cancellationToken = default)
    {
        var ticketId = Guid.Empty;
        ticketId = await ticketing.CreateTicket(metadata, cancellationToken);

        using(var scope = Tracer.Instance.StartActive("CreateTicket", new SpanCreationSettings { Parent = new SpanContext(null, ticketId.ToULong()) }))
        {
            scope.Span.SetTag("ticketId", ticketId.ToString("D"));
            scope.Span.SetTag("status", "Created");
            scope.Span.SetTag("createdTimestamp", DateTime.UtcNow.ToString("o"));
        }

        return ticketId;
    }

    public static async Task<IEnumerable<Ticket>> GetAll(ITicketing ticketing,
        CancellationToken cancellationToken = default)
        => await ticketing.GetAll(cancellationToken);

    public static async Task<Ticket?> Get(Guid ticketId, ITicketing ticketing,
        CancellationToken cancellationToken = default)
        => await ticketing.Get(ticketId, cancellationToken);

    public static async Task Pending(Guid ticketId, ITicketing ticketing, CancellationToken cancellationToken = default)
    {
        using var scope = Tracer.Instance.StartActive("SetTicketPending");
        await ticketing.Pending(ticketId, cancellationToken);

        scope.Span.SetTag("ticketId", ticketId.ToString("D"));
        scope.Span.SetTag("status", "Pending");
        scope.Span.SetTag("lastChangedTimestamp", DateTime.UtcNow.ToString("o"));
    }

    public static async Task Complete(
        Guid ticketId,
        TicketResult result,
        ITicketing ticketing,
        CancellationToken cancellationToken = default)
    {
        using var scope = Tracer.Instance.StartActive("CompleteTicket");
        await ticketing.Complete(ticketId, result, cancellationToken);

        scope.Span.SetTag("ticketId", ticketId.ToString("D"));
        scope.Span.SetTag("status", "Complete");
        scope.Span.SetTag("lastChangedTimestamp", DateTime.UtcNow.ToString("o"));
    }

    public static async Task Error(
        Guid ticketId,
        TicketError error,
        ITicketing ticketing,
        CancellationToken cancellationToken = default)
    {
        using var scope = Tracer.Instance.StartActive("ErrorTicket");
        await ticketing.Error(ticketId, error, cancellationToken);

        scope.Span.SetTag("ticketId", ticketId.ToString("D"));
        scope.Span.SetTag("status", "Error");
        scope.Span.SetTag("lastChangedTimestamp", DateTime.UtcNow.ToString("o"));
    }

    public static async Task Delete(Guid ticketId, ITicketing ticketing, CancellationToken cancellationToken = default)
    {
        using var scope = Tracer.Instance.StartActive("DeleteTicket");
        await ticketing.Delete(ticketId, cancellationToken);

        scope.Span.SetTag("ticketId", ticketId.ToString("D"));
        scope.Span.SetTag("status", "Deleted");
        scope.Span.SetTag("lastChangedTimestamp", DateTime.UtcNow.ToString("o"));
    }
}
