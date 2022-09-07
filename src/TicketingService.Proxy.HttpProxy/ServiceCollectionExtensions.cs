namespace TicketingService.Proxy.HttpProxy;

using Microsoft.Extensions.DependencyInjection;
using Abstractions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddHttpProxyTicketing(this IServiceCollection services)
    {
        services.AddScoped<ITicketing, HttpProxyTicketing>();
        return services;
    }
}
