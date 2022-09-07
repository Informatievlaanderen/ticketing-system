namespace TicketingService.Abstractions;

using System;

public interface ITicketingUrl
{
    Uri For(Guid ticketId);
}
