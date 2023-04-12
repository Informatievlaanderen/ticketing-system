namespace TicketingService.Abstractions;

using System;
using System.Runtime.Serialization;
using System.Text.Json;
using System.Text.Json.Serialization;

/// <summary>
/// Het resultaat van het ticket in json formaat.
/// </summary>
[DataContract(Name = "TicketResult")]
public record TicketResult
{
    [JsonInclude]
    [JsonPropertyName("json")]
    [DataMember(Name = "json")]
    public string? ResultAsJson { get; set; } = null;

    public TicketResult(object? result)
    {
        if (result is not null)
        {
            ResultAsJson = result is Array
                ? JsonSerializer.Serialize(result)
                : JsonSerializer.Serialize(new[] { result });
        }
    }

    public TicketResult()
    { }
}

public record TicketError(string ErrorMessage, string ErrorCode);
