namespace TicketingService.IntegrationTests;

using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Security.Claims;
using System.Threading.Tasks;
using Abstractions;
using Be.Vlaanderen.Basisregisters.DockerUtilities;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using Storage.PgSqlMarten;
using Xunit;
using static JwtTokenHelper;
using JsonSerializer = System.Text.Json.JsonSerializer;

public class TicketingServiceTests
{

    [Fact]
    public async Task CreateGetUpdatePendingErrorCompleteDelete()
    {
        // start postgres
        using var _ = DockerComposer.Compose("postgres_test.yml", "ticketing-system-integration-tests");

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
            var s = await client.GetStringAsync($"/tickets/{ticketId:D}");
            var ticket = JsonConvert.DeserializeObject<Ticket>(s);
            Assert.NotNull(ticket);
            Assert.Equal(TicketStatus.Created, ticket!.Status);
            Assert.NotEqual(default, ticket.Created);
            Assert.NotEqual(default, ticket.LastModified);

            var ticketCreatedOn = ticket.Created;

            Assert.Equal(originator, ticket.Metadata[originator]);

            // pending
            var request = new HttpRequestMessage(HttpMethod.Put, $"/tickets/{ticketId}/pending");
            await client.SendAsync(request);
            s = await client.GetStringAsync($"/tickets/{ticketId:D}");
            ticket = JsonConvert.DeserializeObject<Ticket>(s);
            Assert.NotNull(ticket);
            Assert.Equal(TicketStatus.Pending, ticket!.Status);
            Assert.Equal(ticketCreatedOn, ticket.Created);
            Assert.True(ticket.Created < ticket.LastModified);

            var ticketLastModified = ticket.LastModified;

            // error
            var ticketError = new TicketError("mockErrorMessage", "mockErrorCode");
            request = new HttpRequestMessage(HttpMethod.Put, $"/tickets/{ticketId}/error");
            request.Headers.Authorization = new AuthenticationHeaderValue(jwtToken);
            request.Content = JsonContent.Create(ticketError);
            await client.SendAsync(request);

            // get
            s = await client.GetStringAsync($"/tickets/{ticketId:D}");
            ticket = JsonConvert.DeserializeObject<Ticket>(s);
            Assert.NotNull(ticket);
            Assert.Equal(TicketStatus.Error, ticket!.Status);
            Assert.Equal(new TicketResult(ticketError), ticket.Result);

            // errors
            var ticketErrors = new TicketError(new[]
            {
                new TicketError("mockErrorMessage1", "mockErrorCode1"),
                new TicketError("mockErrorMessage2", "mockErrorCode2"),
                new TicketError("mockErrorMessage3", "mockErrorCode3")
            });
            request = new HttpRequestMessage(HttpMethod.Put, $"/tickets/{ticketId}/error");
            request.Headers.Authorization = new AuthenticationHeaderValue(jwtToken);
            request.Content = JsonContent.Create(ticketErrors);
            await client.SendAsync(request);

            // get
            s = await client.GetStringAsync($"/tickets/{ticketId:D}");
            ticket = JsonConvert.DeserializeObject<Ticket>(s);
            Assert.NotNull(ticket);
            Assert.Equal(TicketStatus.Error, ticket!.Status);
            Assert.Equivalent(new TicketResult(ticketErrors), ticket.Result);

            // complete
            const string complete = "Complete";
            request = new HttpRequestMessage(HttpMethod.Put, $"/tickets/{ticketId}/complete");
            request.Content = JsonContent.Create(new TicketResult(complete));
            await client.SendAsync(request);

            // get
            s = await client.GetStringAsync($"/tickets/{ticketId:D}");
            ticket = JsonConvert.DeserializeObject<Ticket>(s);
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
            var r = await client.GetAsync($"/tickets/{ticketId:D}");
            Assert.Equal(HttpStatusCode.NotFound, r.StatusCode);
        }
    }
}
