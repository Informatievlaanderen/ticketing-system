namespace TicketingService.Abstractions;

using System;
using System.Collections.Generic;
using System.Linq;
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
    public string? ResultAsJson { get; set; }

    public TicketResult(object? result)
    {
        ResultAsJson = result is not null ? JsonSerializer.Serialize(result) : null;
    }

    public TicketResult()
    {
    }
}

public record TicketError
{
    private IList<TicketError>? _errors;

    // init needed for minimal api (de)serialization
    public string ErrorMessage { get; init; }
    public string ErrorCode { get; init; }

    public IReadOnlyCollection<TicketError>? Errors
    {
        get => _errors?.ToList().AsReadOnly();
        set => _errors = value?.ToList();
    }
    public int? ErrorCount => _errors?.Count;

    public TicketError(string errorMessage, string errorCode)
        : this()
    {
        ErrorMessage = errorMessage;
        ErrorCode = errorCode;
    }

    public TicketError(IList<TicketError> errors)
        : this()
    {
        _errors = new List<TicketError>();
        if (errors.Count == 0)
        {
            throw new ArgumentException("Value cannot be an empty collection.", nameof(errors));
        }

        _errors = errors;

        ErrorCode = errors.First().ErrorCode;
        ErrorMessage = errors.First().ErrorMessage;
    }

    // Needed for minimal api (de)serialization
    public TicketError()
    {
        _errors = null;
        ErrorMessage = string.Empty;
        ErrorCode = string.Empty;
    }
}
