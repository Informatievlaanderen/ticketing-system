namespace TicketingService.Proxy.HttpProxy;

using System;
using Abstractions;
using Microsoft.Extensions.DependencyInjection;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddHttpProxyTicketing(
        this IServiceCollection services,
        string baseUrl)
    {
        services.AddHttpClient<ITicketing, HttpProxyTicketing>(c =>
        {
            c.BaseAddress = new Uri(baseUrl.TrimEnd('/'));
        });

        return services;
    }
}
