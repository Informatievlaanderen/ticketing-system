namespace TicketingService.Storage.InMemory;

using Abstractions;
using Microsoft.Extensions.DependencyInjection;

public static class ServiceCollectionExtensions
{
    public static void AddInMemoryTicketing(this IServiceCollection services, string connectionString)
        => services.AddSingleton<ITicketing, InMemoryTicketing>();
}
