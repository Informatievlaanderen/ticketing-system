namespace TicketingService.Abstractions;

using System;
using System.Collections.Generic;

public record Ticket(
    Guid TicketId,
    TicketStatus Status,
    IDictionary<string, string> Metadata,
    TicketResult? Result = null)
{
    /// <summary>
    /// De unieke identificator van het ticket.
    /// </summary>
    public Guid TicketId { get; set; } = TicketId;

    /// <summary>
    /// De status van het ticket.
    /// </summary>
    public TicketStatus Status { get; set; } = Status;

    /// <summary>
    /// De resultaat van het ticket in json formaat.
    /// </summary>
    public TicketResult? Result { get; set; } = Result;

    /// <summary>
    /// De datum waarop het ticket is aangemaakt.
    /// </summary>
    public DateTimeOffset Created { get; init; } = DateTimeOffset.UtcNow;

    /// <summary>
    /// De datum waarop het ticket laatst is aangepast.
    /// </summary>
    public DateTimeOffset LastModified { get; set; } = DateTimeOffset.UtcNow;

    public void ChangeStatus(TicketStatus newStatus, TicketResult? result = null)
    {
        Status = newStatus;
        LastModified = DateTimeOffset.UtcNow;

        if (Status is TicketStatus.Complete or TicketStatus.Error && result is not null)
        {
            Result = result;
        }
    }
}
