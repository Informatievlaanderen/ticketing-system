namespace TicketingService.Endpoints;

using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Abstractions;

public static class Handlers
{
    public static async Task<Guid> Create(string originator, ITicketing ticketing, CancellationToken cancellationToken = default) => await ticketing.CreateTicket(originator, cancellationToken);

    public static async Task<IEnumerable<Ticket>> GetAll(ITicketing ticketing, CancellationToken cancellationToken = default) => await ticketing.GetAll(cancellationToken);

    public static async Task<Ticket?> Get(Guid ticketId, ITicketing ticketing, CancellationToken cancellationToken = default) => await ticketing.Get(ticketId, cancellationToken);

    public static async Task Pending(Guid ticketId, ITicketing ticketing, CancellationToken cancellationToken = default) => await ticketing.Pending(ticketId, cancellationToken);

    public static async Task Complete(Guid ticketId, TicketResult result, ITicketing ticketing, CancellationToken cancellationToken = default) => await ticketing.Complete(ticketId, result, cancellationToken);

    public static async Task Delete(Guid ticketId, ITicketing ticketing, CancellationToken cancellationToken = default) => await ticketing.Delete(ticketId, cancellationToken);
}
