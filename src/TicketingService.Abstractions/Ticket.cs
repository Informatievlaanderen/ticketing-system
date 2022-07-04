namespace TicketingService.Abstractions;

using System;

public class Ticket
{
    public Guid TicketId { get; }
    public string Originator { get; }
    public TicketStatus Status { get; set; }
    public string Body { get; }

    public Ticket(Guid ticketId, string originator, TicketStatus status, string body)
    {
        TicketId = ticketId;
        Originator = originator;
        Status = status;
        Body = body;
    }
}
