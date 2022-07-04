namespace TicketingService.IntegrationTests;

using System;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Testing;
using Abstractions;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using Storage.InMemory;
using Xunit;

public class TicketingServiceTests
{
    [Fact]
    public async Task CreateGetUpdatePendingComplete()
    {
        var application = new WebApplicationFactory<Program>()
            .WithWebHostBuilder(builder =>
            {
                builder.ConfigureServices(services => services.AddSingleton<ITicketing, InMemoryTicketing>());
            });

        var client = application.CreateClient();

        const string originator = "originator";
        var response = await client.PostAsync($"/tickets/create/{originator}", new StringContent(""), CancellationToken.None);
        var ticketId = JsonConvert.DeserializeObject<Guid>(await response.Content.ReadAsStringAsync(CancellationToken.None));
        
        response = await client.GetAsync($"/tickets/{ticketId:D}", CancellationToken.None);
        var ticket = JsonConvert.DeserializeObject<Ticket>(await response.Content.ReadAsStringAsync(CancellationToken.None));
        Assert.NotNull(ticket);
        Assert.Equal(TicketStatus.Created, ticket!.Status);
        Assert.Equal(originator, ticket.Originator);

        await client.PutAsync($"/tickets/{ticketId}/pending", new StringContent(""), CancellationToken.None);
        response = await client.GetAsync($"/tickets/{ticketId:D}", CancellationToken.None);
        ticket = JsonConvert.DeserializeObject<Ticket>(await response.Content.ReadAsStringAsync(CancellationToken.None));
        Assert.NotNull(ticket);
        Assert.Equal(TicketStatus.Pending, ticket!.Status);

        await client.PutAsync($"/tickets/{ticketId}/complete", new StringContent(""), CancellationToken.None);
        response = await client.GetAsync($"/tickets/{ticketId:D}", CancellationToken.None);
        ticket = JsonConvert.DeserializeObject<Ticket>(await response.Content.ReadAsStringAsync(CancellationToken.None));
        Assert.NotNull(ticket);
        Assert.Equal(TicketStatus.Complete, ticket!.Status);
    }
}
