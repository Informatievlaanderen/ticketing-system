namespace TicketingService.IntegrationTests;

using System;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text.Json;
using System.Threading.Tasks;
using Abstractions;
using Microsoft.AspNetCore.Mvc.Testing;
using Storage.PgSqlMarten;
using Xunit;

public class TicketingServiceTests
{
    [Fact]
    public async Task CreateGetUpdatePendingCompleteDelete()
    {
        const string connectionString = "Server=127.0.0.1;Port=5432;Database=tickets;User Id=postgres;Password=mysecretpassword;";

        var application = new WebApplicationFactory<Program>()
            .WithWebHostBuilder(builder =>
            {
                builder.ConfigureServices(services => services.AddMartenTicketing(connectionString));
            });

        var client = application.CreateClient();

        // create
        const string originator = "originator";
        var ticketId = Guid.Empty;
        var response = await client.PostAsync($"/tickets/create/{originator}", new ReadOnlyMemoryContent(null));
        if (response.IsSuccessStatusCode)
        {
            ticketId = await JsonSerializer.DeserializeAsync<Guid>(await response.Content.ReadAsStreamAsync());
        }
        try
        {
            // get
            var ticket = await client.GetFromJsonAsync<Ticket>($"/tickets/{ticketId:D}");
            Assert.NotNull(ticket);
            Assert.Equal(TicketStatus.Created, ticket!.Status);
            Assert.Equal(originator, ticket.Originator);

            // pending
            await client.PutAsync($"/tickets/{ticketId}/pending", new ReadOnlyMemoryContent(null));
            ticket = await client.GetFromJsonAsync<Ticket>($"/tickets/{ticketId:D}");
            Assert.NotNull(ticket);
            Assert.Equal(TicketStatus.Pending, ticket!.Status);

            // complete
            const string complete = "Complete";
            await client.PutAsJsonAsync($"/tickets/{ticketId}/complete", new TicketResult(complete));

            // get
            ticket = await client.GetFromJsonAsync<Ticket>($"/tickets/{ticketId:D}");
            Assert.NotNull(ticket);
            Assert.Equal(TicketStatus.Complete, ticket!.Status);
            Assert.Equal(new TicketResult(complete), ticket.Result);
        }
        finally
        {
            // delete
            await client.DeleteAsync($"/tickets/{ticketId}");
            var ticket = await client.GetFromJsonAsync<Ticket>($"/tickets/{ticketId:D}");
            Assert.Null(ticket);
        }
    }
}
