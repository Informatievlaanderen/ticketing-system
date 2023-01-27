namespace TicketingService.Storage.PgSqlMarten;

using System.Text.Json;
using System.Text.Json.Serialization;
using Abstractions;
using Marten;
using Marten.Schema.Identity;
using Marten.Services;
using Marten.Services.Json;
using Microsoft.Extensions.DependencyInjection;
using Weasel.Core;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddMartenTicketing(this IServiceCollection services, string connectionString)
    {
        services.AddMarten(options =>
        {
            options.Connection(connectionString);
            options.AutoCreateSchemaObjects = AutoCreate.All;
            options.Schema.For<Ticket>()
                .IdStrategy(new CombGuidIdGeneration())
                .Identity(x => x.TicketId);

            options.CreateDatabasesForTenants(configure => configure.ForTenant()
                .CheckAgainstPgDatabase()
                .WithEncoding("UTF-8")
                .ConnectionLimit(-1));

            options.UseDefaultSerialization(serializerType: SerializerType.SystemTextJson);

            // Optionally configure the serializer directly
            var systemTextJsonSerializer = new SystemTextJsonSerializer
            {
                // Optionally override the enum storage
                EnumStorage = EnumStorage.AsString,

                // Optionally override the member casing
                Casing = Casing.CamelCase,
            };

            systemTextJsonSerializer.Customize(serializerOptions =>
            {
                serializerOptions.DictionaryKeyPolicy = JsonNamingPolicy.CamelCase;
                serializerOptions.Converters.Add(new JsonStringEnumConverter(JsonNamingPolicy.CamelCase));
            });

            options.Serializer(systemTextJsonSerializer);
        });

        services.AddSingleton<ITicketing, MartenTicketing>();
        return services;
    }
}
