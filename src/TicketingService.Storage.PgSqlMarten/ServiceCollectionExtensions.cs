namespace TicketingService.Storage.PgSqlMarten;

using Abstractions;
using Marten;
using Marten.Schema.Identity;
using Microsoft.Extensions.DependencyInjection;
using Weasel.Core;

public static class ServiceCollectionExtensions
{
    public static void AddMartenTicketing(this IServiceCollection services, string connectionString)
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
                .WithOwner("postgres")
                .WithEncoding("UTF-8")
                .ConnectionLimit(-1));
        });
        
        services.AddSingleton<ITicketing, MartenTicketing>();
    }
}
