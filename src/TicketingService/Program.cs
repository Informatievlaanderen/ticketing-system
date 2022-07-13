using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using TicketingService.Endpoints;
using TicketingService.Storage.PgSqlMarten;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddMartenTicketing(builder.Configuration.GetConnectionString("Marten"));
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// map endpoints
app.MapPost("/tickets/create/{originator}", Handlers.Create);
app.MapGet("/tickets/{ticketId:guid}", Handlers.Get);
app.MapPut("/tickets/{ticketId:guid}/pending", Handlers.Pending);
app.MapPut("/tickets/{ticketId:guid}/complete", Handlers.Complete);
app.MapDelete("/tickets/{ticketId:guid}", Handlers.Delete);
app.MapGet("/tickets", Handlers.GetAll);

app.UseSwagger();
app.UseSwaggerUI();

app.Run();
