namespace TicketingService.Abstractions;

using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

public record Ticket(
    Guid TicketId,
    TicketStatus Status,
    IDictionary<string, string> Metadata,
    TicketResult? Result = null)
{
    public TicketStatus Status { get; set; } = Status;
    public TicketResult? Result { get; set; } = Result;
}

public enum TicketStatus
{
    Created,
    Pending,
    Complete
}

[JsonConverter(typeof(TicketResultJsonConverter))]
public record TicketResult(object? Result);
