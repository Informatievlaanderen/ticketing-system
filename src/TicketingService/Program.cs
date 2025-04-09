using System;
using System.Net;
using System.Net.Mime;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading;
using Be.Vlaanderen.Basisregisters.AspNetCore.Mvc.Middleware;
using Destructurama;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Json;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.Extensions.Logging;
using Serilog;
using Serilog.Debugging;
using TicketingService.Abstractions;
using TicketingService.Endpoints;
using TicketingService.Extensions;
using TicketingService.Storage.PgSqlMarten;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

// add configuration
builder.Configuration
    .AddJsonFile("appsettings.json")
    .AddJsonFile($"appsettings.{builder.Environment.EnvironmentName.ToLowerInvariant()}.json", optional: true, reloadOnChange: false)
    .AddJsonFile($"appsettings.{Environment.MachineName.ToLowerInvariant()}.json", optional: true, reloadOnChange: false)
    .AddEnvironmentVariables()
    .AddCommandLine(args);

var connectionString = builder.Configuration.GetConnectionString("Marten")!;

// add dependencies
var healthOptions = new HealthOptions();
builder.Configuration.Bind("Health", healthOptions);
builder.Services
    .AddMartenTicketing(connectionString)
    .AddHealthChecksFromConfiguration(healthOptions, connectionString);

// add logging
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

// add security
builder.Services
    .AddCors()
    .AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer();
builder.Services.AddAuthorization();

builder.Services.Configure<JsonOptions>(options =>
{
    options.SerializerOptions.Converters.Add(new JsonStringEnumConverter(JsonNamingPolicy.CamelCase));
    options.SerializerOptions.DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull;
});

// add swagger
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();
app
    .UseMiddleware<AddVersionHeaderMiddleware>()
    .UseHealthChecks(new PathString("/tickets/health"), new HealthCheckOptions
    {
        ResultStatusCodes =
        {
            [HealthStatus.Healthy] = StatusCodes.Status200OK,
            [HealthStatus.Degraded] = StatusCodes.Status200OK,
            [HealthStatus.Unhealthy] = StatusCodes.Status503ServiceUnavailable
        }
    })
    .UseCors()
    .UseAuthentication()
    .UseAuthorization();

// map endpoints
app.MapPost("/tickets/create", Handlers.Create)
    .Produces((int)HttpStatusCode.OK, contentType: MediaTypeNames.Application.Json)
    .ExcludeFromDescription();
app.MapGet("/tickets/{ticketId:guid}", async (Guid ticketId, ITicketing ticketingService, CancellationToken cancellationToken) =>
    {
        var result = await Handlers.Get(ticketId, ticketingService, cancellationToken);
        return result is null
            ? Results.NotFound()
            : Results.Ok(result);
    })
    .AllowAnonymous()
    .Produces<Ticket?>(contentType: MediaTypeNames.Application.Json);
app.MapPut("/tickets/{ticketId:guid}/pending", Handlers.Pending)
    .Produces((int)HttpStatusCode.OK, contentType: MediaTypeNames.Application.Json)
    .ExcludeFromDescription();
app.MapPut("/tickets/{ticketId:guid}/complete", Handlers.Complete)
    .Accepts<TicketResult>(contentType: MediaTypeNames.Application.Json)
    .Produces((int)HttpStatusCode.OK, contentType: MediaTypeNames.Application.Json)
    .ExcludeFromDescription();
app.MapPut("/tickets/{ticketId:guid}/error", Handlers.Error)
    .Accepts<TicketError>(contentType: MediaTypeNames.Application.Json)
    .Produces((int)HttpStatusCode.OK, contentType: MediaTypeNames.Application.Json)
    .ExcludeFromDescription();
app.MapDelete("/tickets/{ticketId:guid}", Handlers.Delete)
    .Produces((int)HttpStatusCode.OK, contentType: MediaTypeNames.Application.Json)
    .ExcludeFromDescription();

// use swagger
app.UseSwagger();
app.UseSwaggerUI();

app.Run();
