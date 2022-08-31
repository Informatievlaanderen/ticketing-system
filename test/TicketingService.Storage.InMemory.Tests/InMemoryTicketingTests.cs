namespace TicketingService.Storage.InMemory.Tests;

using System;
using System.Threading.Tasks;
using Abstractions;
using Xunit;

public class InMemoryTicketingTests
{
    [Fact]
    public async Task CreateGetUpdatePendingCompleteDelete()
    {
        var ticketing = new InMemoryTicketing() as ITicketing;

        // create
        const string originator = "originator";
        var ticketId = await ticketing.CreateTicket(originator);
        try
        {
            // get
            var ticket = await ticketing.Get(ticketId);
            Assert.NotNull(ticket);
            Assert.Equal(TicketStatus.Created, ticket!.Status);
            Assert.Equal(originator, ticket.Originator);

            // pending
            await ticketing.Pending(ticketId);
            ticket = await ticketing.Get(ticketId);
            Assert.NotNull(ticket);
            Assert.Equal(TicketStatus.Pending, ticket!.Status);

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
}
