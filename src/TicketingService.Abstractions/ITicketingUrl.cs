namespace TicketingService.Abstractions;

using System;

public interface ITicketingUrl
{
    string Scheme { get; }
    string Host { get; }
    string PathBase { get; }

    string For(Guid ticketId);
}
