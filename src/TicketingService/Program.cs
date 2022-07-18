using System.Net;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using TicketingService.Abstractions;
using TicketingService.Endpoints;
using TicketingService.Storage.PgSqlMarten;

var builder = WebApplication.CreateBuilder(args);

// add configuration
builder.Configuration
    .AddJsonFile("appsettings.json")
    .AddJsonFile("appsettings.*.json", true)
    .AddEnvironmentVariables();

// add ticketing
builder.Services.AddMartenTicketing(builder.Configuration.GetConnectionString("Marten"));

// add security
builder.Services.AddCors();
builder.Services
    .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer();
builder.Services.AddAuthorization(/*configure =>
{
  configure.FallbackPolicy = new AuthorizationPolicyBuilder()
      .AddAuthenticationSchemes(JwtBearerDefaults.AuthenticationScheme)
      .RequireAuthenticatedUser()
      .Build();
}*/);

// add swagger
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();
app
    .UseCors()
    .UseAuthentication()
    .UseAuthorization();

// map endpoints
app.MapPost("/tickets/create/{originator}", Handlers.Create)
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
app.MapDelete("/tickets/{ticketId:guid}", Handlers.Delete)
    .Produces((int)HttpStatusCode.OK)
    .ExcludeFromDescription();

// use swagger
app.UseSwagger();
app.UseSwaggerUI();

app.Run();
