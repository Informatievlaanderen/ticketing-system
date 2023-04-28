namespace TicketingService.Endpoints;

using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Abstractions;
using Elastic.Apm;
using Microsoft.AspNetCore.Mvc;

public static class Handlers
{
    public static async Task<Guid> Create([FromBody] IDictionary<string, string>? metadata, ITicketing ticketing, CancellationToken cancellationToken = default)
    {
        var transaction = Agent.Tracer.StartTransaction("TicketLifespan", "CreateTicket");
        var createSpan = transaction.StartSpan("Create", "Operation");

        var ticketId = await ticketing.CreateTicket(metadata, cancellationToken);

        transaction.SetLabel("TicketId", ticketId.ToString());

        createSpan.End();
        transaction.End();

        return ticketId;
    }

    public static async Task<IEnumerable<Ticket>> GetAll(ITicketing ticketing, CancellationToken cancellationToken = default)
        => await ticketing.GetAll(cancellationToken);

    public static async Task<Ticket?> Get(Guid ticketId, ITicketing ticketing, CancellationToken cancellationToken = default)
        => await ticketing.Get(ticketId, cancellationToken);

    public static async Task Pending(Guid ticketId, ITicketing ticketing, CancellationToken cancellationToken = default)
        => await ticketing.Pending(ticketId, cancellationToken);

    public static async Task Complete(Guid ticketId, TicketResult result, ITicketing ticketing, CancellationToken cancellationToken = default)
    {
        var transaction = Agent.Tracer.StartTransaction("TicketLifespan", "CompleteTicket");
        var completeSpan = transaction.StartSpan("Complete", "Operation");
        transaction.SetLabel("TicketId", ticketId.ToString());

        await ticketing.Complete(ticketId, result, cancellationToken);

        completeSpan.End();
        transaction.End();
    }

    public static async Task Error(Guid ticketId, TicketError error, ITicketing ticketing, CancellationToken cancellationToken = default)
    {
        var transaction = Agent.Tracer.StartTransaction("TicketLifespan", "CompleteTicket");
        var errorSpan = transaction.StartSpan("Error", "Operation");
        transaction.SetLabel("TicketId", ticketId.ToString());

        await ticketing.Error(ticketId, error, cancellationToken);

        errorSpan.End();
        transaction.End();
    }

    public static async Task Delete(Guid ticketId, ITicketing ticketing, CancellationToken cancellationToken = default)
    {
        var transaction = Agent.Tracer.StartTransaction("TicketLifespan", "CompleteTicket");
        var createSpan = transaction.StartSpan("Delete", "Operation");
        transaction.SetLabel("TicketId", ticketId.ToString());

        await ticketing.Delete(ticketId, cancellationToken);

        createSpan.End();
        transaction.End();
    }
}
