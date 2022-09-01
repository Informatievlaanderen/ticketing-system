namespace TicketingService.IntegrationTests;

using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Security.Claims;
using System.Text.Json;
using System.Threading.Tasks;
using Abstractions;
using ContainerHelper;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using Storage.PgSqlMarten;
using Xunit;
using static JwtTokenHelper;

public class TicketingServiceTests
{

    [Fact]
    public async Task CreateGetUpdatePendingCompleteDelete()
    {
        // start postgres
        var composeFileName = Path.Combine(Directory.GetCurrentDirectory(), "postgres_test.yml");
        using var _ = Container.Compose(composeFileName, "postgres_test", "5433", "tcp");

        // construct claims identity
        var claimsIdentity = new ClaimsIdentity(new[] { new Claim("internal", "true") });
        var jwtToken = CreateJwtToken(claimsIdentity);

        var application = new WebApplicationFactory<Program>()
          .WithWebHostBuilder(builder =>
          {
              const string connectionString = "Host=localhost;Port=5433;Database=tickets;Username=postgres;Password=postgres";
              builder.ConfigureServices(services => services
                .AddScoped(_ => new ClaimsPrincipal(claimsIdentity))
                .AddMartenTicketing(connectionString));
          });

        var client = application.CreateClient();
        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(jwtToken);

        // create ticket
        const string originator = "originator";
        var ticketId = Guid.Empty;
        var response = await client.PostAsync("/tickets/create", JsonContent.Create(new Dictionary<string, string> { { originator, originator } }));

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
            Assert.NotEqual(default, ticket.Created);
            Assert.NotEqual(default, ticket.LastModified);

            var ticketCreatedOn = ticket.Created;

            Assert.Equal(originator, ticket.Metadata[originator]);

            // pending
            var request = new HttpRequestMessage(HttpMethod.Put, $"/tickets/{ticketId}/pending");
            await client.SendAsync(request);
            ticket = await client.GetFromJsonAsync<Ticket>($"/tickets/{ticketId:D}");
            Assert.NotNull(ticket);
            Assert.Equal(TicketStatus.Pending, ticket!.Status);
            Assert.Equal(ticketCreatedOn, ticket.Created);
            Assert.True(ticket.Created < ticket.LastModified);

            var ticketLastModified = ticket.LastModified;

            // complete
            const string complete = "Complete";
            request = new HttpRequestMessage(HttpMethod.Put, $"/tickets/{ticketId}/complete");
            request.Content = JsonContent.Create(new TicketResult(complete));
            await client.SendAsync(request);

            // get
            ticket = await client.GetFromJsonAsync<Ticket>($"/tickets/{ticketId:D}");
            Assert.NotNull(ticket);
            Assert.Equal(TicketStatus.Complete, ticket!.Status);
            Assert.Equal(new TicketResult(complete), ticket.Result);
            Assert.True(ticketLastModified < ticket.LastModified);
        }
        finally
        {
            // delete
            var request = new HttpRequestMessage(HttpMethod.Delete, $"/tickets/{ticketId:D}");
            await client.SendAsync(request);
            var ticket = await client.GetFromJsonAsync<Ticket>($"/tickets/{ticketId:D}");
            Assert.Null(ticket);
        }
    }
}
