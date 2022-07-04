using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using TicketingService.Abstractions;
using TicketingService.Endpoints;
using TicketingService.Storage.InMemory;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddSingleton<ITicketing, InMemoryTicketing>();

var app = builder.Build();

// map endpoints
app.MapPost("/tickets/create/{originator}", Handlers.Create);
app.MapGet("/tickets/{ticketId:guid}", Handlers.Get);
app.MapPut("/tickets/{ticketId:guid}/pending", Handlers.Pending);
app.MapPut("/tickets/{ticketId:guid}/complete", Handlers.Complete);

app.Run();
