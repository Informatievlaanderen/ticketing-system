namespace TicketingService.Abstractions;

using System;
using System.Threading.Tasks;

public interface ITicketing
{
    Task<Guid> CreateTicket(string originator);
    Task Pending(Guid ticketId);
    Task Complete(Guid ticketId, object? result);
    Task<Ticket?> Get(Guid ticketId);
}
