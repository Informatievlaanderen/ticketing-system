namespace TicketingService.Abstractions;

using System;
using System.Text.Json.Serialization;

public record Ticket(Guid TicketId, string Originator, TicketStatus Status, TicketResult? Result = null)
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
