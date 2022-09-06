namespace TicketingService.Abstractions;

using System;
using System.Collections.Generic;
using System.Text.Json;
using System.Text.Json.Serialization;

public record Ticket(
    Guid TicketId,
    TicketStatus Status,
    IDictionary<string, string> Metadata,
    TicketResult? Result = null)
{
    public TicketStatus Status { get; set; } = Status;
    public TicketResult? Result { get; set; } = Result;
    public DateTimeOffset Created { get; init; } = DateTimeOffset.UtcNow;
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

public enum TicketStatus
{
    Created,
    Pending,
    Complete,
    Error
}

public record TicketResult
{
    [JsonInclude]
    public string? ResultAsJson { get; set; }

    public TicketResult(object? result)
    {
        ResultAsJson = result is not null ? JsonSerializer.Serialize(result) : null;
    }

    public TicketResult()
    { }
}

public record TicketError(string ErrorMessage, string ErrorCode);
