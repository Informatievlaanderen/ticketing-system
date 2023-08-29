namespace TicketingService.Abstractions;

using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

public record Metadata(string Registry, string Action);

public record TicketSearch(
    Guid TicketId,
    Metadata Metadata)
{
    public Guid TicketId { get; set; } = TicketId;

    public Metadata Metadata { get; set; } = Metadata;
}

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
    [JsonIgnore(Condition = JsonIgnoreCondition.Never)]
    public TicketStatus Status { get; set; } = Status;

    /// <summary>
    /// De metadata van het ticket.
    /// </summary>
    public IDictionary<string, string> Metadata { get; set; } = Metadata;

    /// <summary>
    /// Het resultaat van het ticket in json formaat.
    /// </summary>
    public TicketResult? Result { get; set; } = Result;

    /// <summary>
    /// De datum en het tijdstip waarop het ticket is aangemaakt (timestamp volgens RFC 3339) (notatie: lokale tijd + verschil t.o.v. UTC).
    /// </summary>
    public DateTimeOffset Created
    {
        get => ToWesternEuropeDateTimeOffset(_created);
        init => _created = value;
    }

    /// <summary>
    /// De datum en het tijdstip waarop het ticket laatst is aangepast (timestamp volgens RFC 3339) (notatie: lokale tijd + verschil t.o.v. UTC).
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
        var utcOffset = TimeZoneInfo.FindSystemTimeZoneById("W. Europe Standard Time").GetUtcOffset(dateTimeOffset);

        if (utcOffset == dateTimeOffset.Offset)
        {
            return dateTimeOffset;
        }

        var dateTime = TimeZoneInfo.ConvertTimeFromUtc(
            dateTimeOffset.DateTime,
            TimeZoneInfo.FindSystemTimeZoneById("W. Europe Standard Time"));

        return new DateTimeOffset(
            dateTime,
            utcOffset);
    }
}
