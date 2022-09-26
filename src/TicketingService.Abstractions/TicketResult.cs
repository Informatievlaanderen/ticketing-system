namespace TicketingService.Abstractions;

using System.Runtime.Serialization;
using System.Text.Json;
using System.Text.Json.Serialization;

public record TicketResult
{
    [JsonInclude]
    [JsonPropertyName("json")]
    [DataMember(Name = "json")]
    public string? ResultAsJson { get; set; }

    public TicketResult(object? result)
    {
        ResultAsJson = result is not null ? JsonSerializer.Serialize(result) : null;
    }

    public TicketResult()
    { }
}

public record TicketError(string ErrorMessage, string ErrorCode);
