namespace TicketingService.Storage.InMemory.Tests;

using System.Threading.Tasks;
using Abstractions;
using Xunit;

public class TicketingTests
{
    [Fact]
    public async Task CreateGetUpdatePendingComplete()
    {
        var ticketing = new InMemoryTicketing() as ITicketing;

        const string originator = "originator";
        var ticketId = await ticketing.CreateTicket(originator);
        var ticket = await ticketing.Get(ticketId);
        Assert.NotNull(ticket);
        Assert.Equal(TicketStatus.Created, ticket!.Status);
        Assert.Equal(originator, ticket.Originator);

        await ticketing.Pending(ticketId);
        ticket = await ticketing.Get(ticketId);
        Assert.NotNull(ticket);
        Assert.Equal(TicketStatus.Pending, ticket!.Status);

        await ticketing.Complete(ticketId);
        ticket = await ticketing.Get(ticketId);
        Assert.NotNull(ticket);
        Assert.Equal(TicketStatus.Complete, ticket!.Status);
    }
}
