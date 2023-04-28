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
        var ticketId = Guid.Empty;
        await Agent.Tracer.CaptureTransaction("CreateTicket", "TicketLifespan", async transaction =>
        {
            ticketId = await ticketing.CreateTicket(metadata, cancellationToken);
            transaction.SetLabel("TicketId", ticketId.ToString());
            transaction.SetLabel("Status", "Created");
            transaction.SetLabel("CreatedTimestamp", DateTime.UtcNow.ToString("o"));
        });
        return ticketId;
    }

    public static async Task<IEnumerable<Ticket>> GetAll(ITicketing ticketing, CancellationToken cancellationToken = default)
        => await ticketing.GetAll(cancellationToken);

    public static async Task<Ticket?> Get(Guid ticketId, ITicketing ticketing, CancellationToken cancellationToken = default)
        => await ticketing.Get(ticketId, cancellationToken);

    public static async Task Pending(Guid ticketId, ITicketing ticketing, CancellationToken cancellationToken = default)
    {
        await Agent.Tracer.CaptureTransaction("PendingTicket", "TicketLifespan", async transaction =>
        {
            await ticketing.Pending(ticketId, cancellationToken);
            transaction.SetLabel("TicketId", ticketId.ToString());
            transaction.SetLabel("Status", "Pending");
            transaction.SetLabel("LastChangedTimestamp", DateTime.UtcNow.ToString("o"));
        });
    }

    public static async Task Complete(Guid ticketId, TicketResult result, ITicketing ticketing, CancellationToken cancellationToken = default)
    {
        await Agent.Tracer.CaptureTransaction("CompleteTicket", "TicketLifespan", async transaction =>
        {
            await ticketing.Complete(ticketId, result, cancellationToken);
            transaction.SetLabel("TicketId", ticketId.ToString());
            transaction.SetLabel("Status", "Complete");
            transaction.SetLabel("LastChangedTimestamp", DateTime.UtcNow.ToString("o"));
        });
    }

    public static async Task Error(Guid ticketId, TicketError error, ITicketing ticketing, CancellationToken cancellationToken = default)
    {
        await Agent.Tracer.CaptureTransaction("ErrorTicket", "TicketLifespan", async transaction =>
        {
            await ticketing.Error(ticketId, error, cancellationToken);
            transaction.SetLabel("TicketId", ticketId.ToString());
            transaction.SetLabel("Status", "Error");
            transaction.SetLabel("LastChangedTimestamp", DateTime.UtcNow.ToString("o"));
        });
    }

    public static async Task Delete(Guid ticketId, ITicketing ticketing, CancellationToken cancellationToken = default)
    {
        await Agent.Tracer.CaptureTransaction("DeleteTicket", "TicketLifespan", async transaction =>
        {
            await ticketing.Delete(ticketId, cancellationToken);
            transaction.SetLabel("TicketId", ticketId.ToString());
            transaction.SetLabel("Status", "Deleted");
            transaction.SetLabel("LastChangedTimestamp", DateTime.UtcNow.ToString("o"));
        });
    }
}
