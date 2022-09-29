namespace TicketingService.Abstractions;

using System;
using System.Collections.Generic;

public record Ticket(
    Guid TicketId,
    TicketStatus Status,
    IDictionary<string, string> Metadata,
    TicketResult? Result = null)
{    
    private readonly DateTimeOffset _created = DateTimeOffset.UtcNow;
    private DateTimeOffset _lastModified = DateTimeOffset.UtcNow;

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
    public DateTimeOffset Created
    {
        get => ToWesternEuropeDateTimeOffset(_created);
        init => _created = value;
    }

    /// <summary>
    /// De datum waarop het ticket laatst is aangepast.
    /// </summary>
    public DateTimeOffset LastModified
    {
        get => ToWesternEuropeDateTimeOffset(_lastModified);
        set => _lastModified = value;
    }

    public void ChangeStatus(TicketStatus newStatus, TicketResult? result = null)
    {
        Status = newStatus;
        LastModified = DateTimeOffset.UtcNow;

        if (Status is TicketStatus.Complete or TicketStatus.Error && result is not null)
        {
            Result = result;
        }
    }

    private static DateTimeOffset ToWesternEuropeDateTimeOffset(DateTimeOffset dateTimeOffset)
    {
        return new DateTimeOffset(dateTimeOffset.DateTime, TimeZoneInfo.FindSystemTimeZoneById("W. Europe Standard Time").GetUtcOffset(dateTimeOffset));
    }
}
