namespace TicketingService.Storage.InMemory.Tests;

using Abstractions;
using Xunit;

public class TicketErrorEqualityTests
{
    [Fact]
    public void ItShouldBeEqualWhenSingular()
    {
        var ticketError1 = new TicketError("ErrorMessage", "ErrorCode");
        var ticketError2 = new TicketError("ErrorMessage", "ErrorCode");

        Assert.Equal(ticketError1, ticketError2);
    }

    [Fact]
    public void ItShouldBeEqualWhenMultiple()
    {
        var ticketError1 = new TicketError(new[]
        {
            new TicketError("ErrorMessage1", "ErrorCode1"),
            new TicketError("ErrorMessage2", "ErrorCode2"),
            new TicketError("ErrorMessage3", "ErrorCode3")
        });
        var ticketError2 = new TicketError(new[]
        {
            new TicketError("ErrorMessage1", "ErrorCode1"),
            new TicketError("ErrorMessage2", "ErrorCode2"),
            new TicketError("ErrorMessage3", "ErrorCode3")
        });

        Assert.Equal(ticketError1, ticketError2);
    }

    [Fact]
    public void ItShouldNotBeEqualWhenErrorCollectionDiffers()
    {
        var ticketError1 = new TicketError(new[]
        {
            new TicketError("ErrorMessage1", "ErrorCode1"),
            new TicketError("ErrorMessage2", "ErrorCode2"),
            new TicketError("ErrorMessage3", "ErrorCode3")
        });
        var ticketError2 = new TicketError("ErrorMessage1", "ErrorCode1");

        Assert.NotEqual(ticketError1, ticketError2);
    }
}
