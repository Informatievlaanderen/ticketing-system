namespace TicketingService.Abstractions;

using System;

public class Ticket
{
    public Guid TicketId { get; }
    public string Originator { get; }
    public TicketStatus Status { get; set; }
    public object? Result { get; set; }

    public Ticket(Guid ticketId, string originator, TicketStatus status, object? result = null)
    {
        TicketId = ticketId;
        Originator = originator;
        Status = status;
        Result = result;
    }
}
