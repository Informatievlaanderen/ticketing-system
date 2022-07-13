namespace TicketingService.Abstractions
{
    using System;
    using System.Text.Json;
    using System.Text.Json.Serialization;

    public class TicketResultJsonConverter : JsonConverter<TicketResult>
    {
        public override TicketResult Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options) => new TicketResult(reader.GetString());

        public override void Write(Utf8JsonWriter writer, TicketResult value, JsonSerializerOptions options) => writer.WriteStringValue(value.Result?.ToString());
    }
}
