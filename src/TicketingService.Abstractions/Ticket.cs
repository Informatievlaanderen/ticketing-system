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
    /// De unieke identificator verkregen uit de edit API’s. Deze informatie bevindt zich in de location header van de response.
    /// </summary>
    public Guid TicketId { get; set; } = TicketId;

    /// <summary>
    /// Identificatie van het event, register, objectId en aggregate welke verantwoordelijk is voor het ticket.
    /// </summary>
    public IDictionary<string, string> Metadata { get; set; } = Metadata;

    /// <summary>
    /// De status van het ticket.
    /// </summary>
    public TicketStatus Status { get; set; } = Status;

    /// <summary>
    /// Bij status ‘Complete' wordt de detail URL van het objectId getoond samen met de bijhorende ETag. Bij status ‘Error’ wordt de ErrorMessage & ErrorCode getoond.
    /// </summary>
    public TicketResult? Result { get; set; } = Result;

    /// <summary>
    /// De versie-identificator waarop het ticket is aangemaakt (timestamp volgens RFC 3339) (notatie: lokale tijd + verschil t.o.v. UTC).
    /// </summary>
    public DateTimeOffset Created
    {
        get => ToWesternEuropeDateTimeOffset(_created);
        init => _created = value;
    }

    /// <summary>
    /// De versie-identificator waarop het ticket het laatst is aangepast (timestamp volgens RFC 3339) (notatie: lokale tijd + verschil t.o.v. UTC).
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
