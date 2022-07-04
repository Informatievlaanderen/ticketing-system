namespace TicketingService.Storage.InMemory;

using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Abstractions;

public class InMemoryTicketing : ITicketing
{
    private readonly IDictionary<Guid, Ticket> _tickets = new Dictionary<Guid, Ticket>();

    public Task<Guid> CreateTicket(string originator)
    {
        var ticketId = Guid.NewGuid();
        _tickets[ticketId] = new Ticket(ticketId, originator, TicketStatus.Created, "");

        return Task.FromResult(ticketId);
    }

    private Task ChangeStatus(Guid ticketId, TicketStatus newStatus, string? body = null)
    {
        if (_tickets.TryGetValue(ticketId, out var ticket))
        {
            ticket.Status = newStatus;
            if (ticket.Status == TicketStatus.Complete && body is not null)
            {
                ticket.Body = body;
            }
        }

        return Task.CompletedTask;
    }
    
    public Task Pending(Guid ticketId) => ChangeStatus(ticketId, TicketStatus.Pending);

    public Task Complete(Guid ticketId, Ticket ticket) => ChangeStatus(ticketId, TicketStatus.Complete, ticket.Body);

    public Task<Ticket?> Get(Guid ticketId) => Task.FromResult(_tickets.TryGetValue(ticketId, out var ticket)
        ? ticket
        : null);
}
