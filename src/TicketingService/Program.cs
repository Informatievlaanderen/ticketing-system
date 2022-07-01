using TicketingService.Abstractions;
using TicketingService.Endpoints;
using TicketingService.Storage.InMemory;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddSingleton<ITicketing, InMemoryTicketing>();

var app = builder.Build();

// map endpoints
app.MapPost("/tickets/create/{originator}", Handlers.Create);
app.MapGet("/tickets/{ticketId}", Handlers.Get);
app.MapPut("/tickets/{ticketId}/pending", Handlers.Pending);
app.MapPut("/tickets/{ticketId}/complete", Handlers.Complete);

app.Run();
