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

public record TicketError : IEquatable<TicketError>
{
    private IList<TicketError>? _errors;

    // init needed for minimal api (de)serialization
    public string ErrorMessage { get; init; }
    public string ErrorCode { get; init; }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public IReadOnlyCollection<TicketError>? Errors
    {
        get => _errors?.ToList().AsReadOnly();
        set => _errors = value?.ToList();
    }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public int? ErrorCount => _errors?.Count;

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public Dictionary<string, object>? Context { get; init; } = null;

    public TicketError(string errorMessage, string errorCode)
        : this()
    {
        ErrorMessage = errorMessage;
        ErrorCode = errorCode;
    }

    public TicketError(string errorMessage, string errorCode, Dictionary<string, object>? context = null)
        : this()
    {
        ErrorMessage = errorMessage;
        ErrorCode = errorCode;
        Context = context;
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
        Context = null;
    }

    public virtual bool Equals(TicketError? other)
    {
        if (ReferenceEquals(this, null)) return false;
        if (ReferenceEquals(this, other)) return true;

        if (_errors is not null && other?._errors is null) return false;
        if (_errors is null && other?._errors is not null) return false;
        if (_errors is null || other?._errors is null) return ErrorMessage.Equals(other?.ErrorMessage) && ErrorCode.Equals(other.ErrorCode) && Context == other.Context;

        var sequenceEqual = _errors.SequenceEqual(other._errors);
        return ErrorMessage.Equals(other.ErrorMessage) && ErrorCode.Equals(other.ErrorCode) && Context == other.Context && sequenceEqual;
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(Errors, ErrorMessage, ErrorCode, Context);
    }
}
