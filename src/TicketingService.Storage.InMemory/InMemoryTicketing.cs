namespace TicketingService.Storage.InMemory;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Abstractions;

public class InMemoryTicketing : ITicketing
{
    private readonly IDictionary<Guid, Ticket> _tickets = new Dictionary<Guid, Ticket>();

    public Task<Guid> CreateTicket(string originator, CancellationToken cancellationToken = default)
    {
        var ticketId = Guid.NewGuid();
        _tickets[ticketId] = new Ticket(ticketId, originator, TicketStatus.Created);

        return Task.FromResult(ticketId);
    }

    public Task<IEnumerable<Ticket>> GetAll(CancellationToken cancellationToken = default) => Task.FromResult(_tickets.Values.AsEnumerable());

    public Task<Ticket?> Get(Guid ticketId, CancellationToken cancellationToken = default) => Task.FromResult(_tickets.TryGetValue(ticketId, out var ticket)
        ? ticket
        : null);

    private Task ChangeStatus(Guid ticketId, TicketStatus newStatus, TicketResult? result = null, CancellationToken cancellationToken = default)
    {
        _ = cancellationToken.IsCancellationRequested;
        if (_tickets.TryGetValue(ticketId, out var ticket))
        {
            ticket.Status = newStatus;
            if (ticket.Status == TicketStatus.Complete && result is not null)
            {
                ticket.Result = result;
            }
        }

        return Task.CompletedTask;
    }
    
    public Task Pending(Guid ticketId, CancellationToken cancellationToken = default) => ChangeStatus(ticketId, TicketStatus.Pending, cancellationToken: cancellationToken);

    public Task Complete(Guid ticketId, TicketResult result, CancellationToken cancellationToken = default) => ChangeStatus(ticketId, TicketStatus.Complete, result, cancellationToken);

    public Task Delete(Guid ticketId, CancellationToken cancellationToken = default)
    {
        _tickets.Remove(ticketId);
        return Task.CompletedTask;
    }
}
