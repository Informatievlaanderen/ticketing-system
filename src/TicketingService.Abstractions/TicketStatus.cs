namespace TicketingService.Abstractions;

/// <summary>
/// De status van het ticket.
/// </summary>
public enum TicketStatus
{
    Created,
    Pending,
    Complete,
    Error
}
