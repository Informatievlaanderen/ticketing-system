namespace TicketingService.Endpoints;

using System;
using System.Threading.Tasks;
using Abstractions;

public static class Handlers
{
    public static async Task<Guid> Create(string originator, ITicketing ticketing) => await ticketing.CreateTicket(originator);

    public static async Task<Ticket?> Get(Guid ticketId, ITicketing ticketing) => await ticketing.Get(ticketId);

    public static async Task Pending(Guid ticketId, ITicketing ticketing) => await ticketing.Pending(ticketId);

    public static async Task Complete(Guid ticketId, ITicketing ticketing) => await ticketing.Complete(ticketId);
}
