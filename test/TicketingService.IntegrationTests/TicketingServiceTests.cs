namespace TicketingService.IntegrationTests;

using System;
using System.Net.Http;
using System.Net.Http.Json;
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

        // create
        const string originator = "originator";
        var response = await client.PostAsync($"/tickets/create/{originator}", new StringContent(""));
        var ticketId = JsonConvert.DeserializeObject<Guid>(await response.Content.ReadAsStringAsync());

        // get
        var ticket = await client.GetFromJsonAsync<Ticket>($"/tickets/{ticketId:D}");
        Assert.NotNull(ticket);
        Assert.Equal(TicketStatus.Created, ticket!.Status);
        Assert.Equal(originator, ticket.Originator);

        // pending
        await client.PutAsync($"/tickets/{ticketId}/pending", new StringContent(""));
        ticket = await client.GetFromJsonAsync<Ticket>($"/tickets/{ticketId:D}");
        Assert.NotNull(ticket);
        Assert.Equal(TicketStatus.Pending, ticket!.Status);

        // complete
        const string complete = "Complete";
        await client.PutAsJsonAsync($"/tickets/{ticketId}/complete", complete);
        ticket = await client.GetFromJsonAsync<Ticket>($"/tickets/{ticketId:D}");
        Assert.NotNull(ticket);
        Assert.Equal(TicketStatus.Complete, ticket!.Status);
        Assert.Equal(complete, ticket.Result!.ToString());
    }
}
