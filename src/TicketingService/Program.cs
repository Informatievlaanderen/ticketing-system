using System;
using System.Net;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using TicketingService.Abstractions;
using TicketingService.Endpoints;
using TicketingService.Extensions;
using TicketingService.Storage.PgSqlMarten;

var builder = WebApplication.CreateBuilder(args);

// add configuration
builder.Configuration
    .AddJsonFile("appsettings.json")
    .AddJsonFile($"appsettings.{builder.Environment.EnvironmentName.ToLowerInvariant()}.json", optional: true, reloadOnChange: false)
    .AddJsonFile($"appsettings.{Environment.MachineName.ToLowerInvariant()}.json", optional: true, reloadOnChange: false)
    .AddEnvironmentVariables()
    .AddCommandLine(args);

var connectionString = builder.Configuration.GetConnectionString("Marten");

// add dependencies
var healthOptions = new HealthOptions();
builder.Configuration.Bind("Health", healthOptions);
builder.Services
    .AddMartenTicketing(connectionString)
    .AddHealthChecksFromConfiguration(healthOptions, connectionString);

// add security
builder.Services
    .AddCors()
    .AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer();
builder.Services.AddAuthorization();

// add swagger
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();
app
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
    .Produces((int)HttpStatusCode.OK)
    .ExcludeFromDescription();
app.MapGet("/tickets", Handlers.GetAll)
    .Produces((int)HttpStatusCode.OK)
    .ExcludeFromDescription();
app.MapGet("/tickets/{ticketId:guid}", Handlers.Get)
    .AllowAnonymous()
    .Produces<Ticket?>();
app.MapPut("/tickets/{ticketId:guid}/pending", Handlers.Pending)
    .Produces((int)HttpStatusCode.OK)
    .ExcludeFromDescription();
app.MapPut("/tickets/{ticketId:guid}/complete", Handlers.Complete)
    .Produces((int)HttpStatusCode.OK)
    .ExcludeFromDescription();
app.MapPut("/tickets/{ticketId:guid}/error", Handlers.Error)
    .Produces((int)HttpStatusCode.OK)
    .ExcludeFromDescription();
app.MapDelete("/tickets/{ticketId:guid}", Handlers.Delete)
    .Produces((int)HttpStatusCode.OK)
    .ExcludeFromDescription();

// use swagger
app.UseSwagger();
app.UseSwaggerUI();

app.Run();
