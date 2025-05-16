namespace TicketingService.Monitoring;

using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;
using Amazon.SimpleNotificationService;
using Be.Vlaanderen.Basisregisters.GrAr.Notifications;
using Destructurama;
using HealthChecks;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.Extensions.Logging;
using Serilog;
using Serilog.Debugging;
using Storage.PgSqlMarten;

public static class StartupExtensions
{
    public static WebApplicationBuilder AddAppSettings(this WebApplicationBuilder builder, string[] args)
    {
        builder.Configuration
            .AddJsonFile("appsettings.json")
            .AddJsonFile($"appsettings.{builder.Environment.EnvironmentName.ToLowerInvariant()}.json", optional: true, reloadOnChange: false)
            .AddJsonFile($"appsettings.{Environment.MachineName.ToLowerInvariant()}.json", optional: true, reloadOnChange: false)
            .AddEnvironmentVariables()
            .AddCommandLine(args);

        return builder;
    }

    public static WebApplicationBuilder AddServices(this WebApplicationBuilder builder)
    {
        var options = builder.GetAppOptions<ConnectionStrings>();

        builder.Services
            .AddEndpointsApiExplorer()
            .AddSwaggerGen()
            .AddMartenTicketing(options.Ticketing)
            .AddHealthChecks()
            .AddNpgSql(_ => options.Ticketing);

        builder.Services.AddAWSService<IAmazonSimpleNotificationService>();
         builder.Services.AddSingleton<INotificationService>(provider =>
         {
             var snsService = provider.GetRequiredService<IAmazonSimpleNotificationService>();
             var topicArn = string.IsNullOrWhiteSpace(builder.Configuration["TopicArn"])
                 ? throw new ArgumentException("Configuration has no TopicArn.")
                 : builder.Configuration["TopicArn"];
             return new NotificationService(snsService, topicArn!);
         });

        builder.Services.AddSingleton<TicketsNotifier>();

        builder.Services.AddHostedService<NotificationBackgroundService>();
        builder.Services.AddHostedService<ExpiredTicketsRemover>();

        builder.Services.AddHealthChecks().AddTypeActivatedCheck<ServiceHealthCheck>(
            name: "NotificationService",
            failureStatus: HealthStatus.Unhealthy,
            tags: ["service", "notificationservice"],
            args: [typeof(NotificationBackgroundService), "NotificationService"])
        .AddTypeActivatedCheck<ServiceHealthCheck>(
            name: "ExpiredTicketsRemover",
            failureStatus: HealthStatus.Unhealthy,
            tags: ["service", "expiredticketsremover"],
            args: [typeof(ExpiredTicketsRemover), "ExpiredTicketsRemover"]);

        return builder;
    }

    public static WebApplicationBuilder AddLogging(this WebApplicationBuilder builder)
    {
        SelfLog.Enable(Console.WriteLine);

        Log.Logger = new LoggerConfiguration()
            .ReadFrom.Configuration(builder.Configuration)
            .Enrich.FromLogContext()
            .Enrich.WithMachineName()
            .Enrich.WithThreadId()
            .Enrich.WithEnvironmentUserName()
            .Destructure.JsonNetTypes()
            .CreateLogger();

        builder.Logging.ClearProviders();
        builder.Logging.AddSerilog(Log.Logger);

        return builder;
    }

    public static WebApplicationBuilder AddOptions<TOptions>(this WebApplicationBuilder builder)
        where TOptions : class
    {
        builder.Services.Configure<TOptions>(builder.Configuration.GetSection(typeof(TOptions).Name));
        return builder;
    }

    public static TOptions GetAppOptions<TOptions>(this WebApplicationBuilder builder)
        where TOptions : class, new()
    {
        var options = new TOptions();
        builder.Configuration.Bind(typeof(TOptions).Name, options);

        var requiredProperties = options
            .GetType()
            .GetProperties()
            .Where(prop => Attribute.IsDefined(prop, typeof(RequiredAttribute)));

        void ThrowArgumentNullException(PropertyInfo p) => throw new ArgumentNullException($"{typeof(TOptions).Name}.{p.Name}");

        foreach (var prop in requiredProperties)
        {
            var obj = prop.GetValue(options, null);

            if (prop.GetValue(options, null) is null)
            {
                ThrowArgumentNullException(prop);
            }

            if (obj is string valstr && string.IsNullOrEmpty(valstr))
            {
                ThrowArgumentNullException(prop);
            }
        }

        return options;
    }
}
