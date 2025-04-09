namespace TicketingService.Storage.PgSqlMarten.Tests;

using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Abstractions;
using Be.Vlaanderen.Basisregisters.DockerUtilities;
using Marten;
using Microsoft.Extensions.DependencyInjection;
using Npgsql;
using Xunit;

public class MartenTicketingTests
{
    [Fact]
    public async Task CreateGetUpdatePendingErrorCompleteDelete()
    {
        using var _ = DockerComposer.Compose("postgres_test.yml", "ticketing-system-marten-integration-tests");

        await CreateDatabase("Host=localhost;Port=5434;Username=postgres;Password=postgres", "tickets");

        // add Marten
        var services = new ServiceCollection();
        services.AddMartenTicketing("Host=localhost;Port=5434;Database=tickets;Username=postgres;Password=postgres");
        var serviceProvider = services.BuildServiceProvider();

        var ticketing = new MartenTicketing(serviceProvider.GetRequiredService<IDocumentStore>()) as ITicketing;

        // create
        const string originator = "originator";
        var ticketId = await ticketing.CreateTicket(new Dictionary<string, string> { { originator, originator } });

        try
        {
            // get
            var ticket = await ticketing.Get(ticketId);
            Assert.NotNull(ticket);
            Assert.Equal(TicketStatus.Created, ticket!.Status);
            Assert.Equal(originator, ticket.Metadata[originator]);

            // pending
            await ticketing.Pending(ticketId);
            ticket = await ticketing.Get(ticketId);
            Assert.NotNull(ticket);
            Assert.Equal(TicketStatus.Pending, ticket!.Status);

            // error
            var ticketError = new TicketError("mockErrorMessage", "mockErrorCode");
            await ticketing.Error(ticketId, ticketError);

            // get
            ticket = await ticketing.Get(ticketId);
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
            await ticketing.Error(ticketId, ticketErrors);

            // get
            ticket = await ticketing.Get(ticketId);
            Assert.NotNull(ticket);
            Assert.Equal(TicketStatus.Error, ticket.Status);
            Assert.Equivalent(new TicketResult(ticketErrors), ticket.Result);
            Assert.DoesNotContain("null", ticket.Result?.ResultAsJson);

            // complete
            const string complete = "Complete";
            await ticketing.Complete(ticketId, new TicketResult(complete));

            // get
            ticket = await ticketing.Get(ticketId);
            Assert.NotNull(ticket);
            Assert.Equal(TicketStatus.Complete, ticket!.Status);
            Assert.Equal(new TicketResult(complete), ticket.Result);
        }
        finally
        {
            // delete
            await ticketing.Delete(ticketId);
            var ticket = await ticketing.Get(ticketId);
            Assert.Null(ticket);
        }
    }

    private async Task CreateDatabase(string connectionString, string database)
    {
        var createDbQuery = $"CREATE DATABASE {database}";

        await using var connection = new NpgsqlConnection(connectionString);
        await using var command = new NpgsqlCommand(createDbQuery, connection);

        var attempt = 1;
        while (attempt <= 5)
        {
            try
            {
                await connection.OpenAsync();
            }
            catch (Exception)
            {
                attempt++;
                await Task.Delay(TimeSpan.FromMilliseconds(200));
            }
        }

        await command.ExecuteNonQueryAsync();
    }
}
