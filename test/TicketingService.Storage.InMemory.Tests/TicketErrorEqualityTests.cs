namespace TicketingService.Storage.InMemory.Tests;

using System.Collections.Generic;
using Abstractions;
using Xunit;

public sealed class TicketErrorEqualityTests
{
    [Fact]
    public void ItShouldBeEqualWhenSingular()
    {
        var ticketError1 = new TicketError("ErrorMessage", "ErrorCode");
        var ticketError2 = new TicketError("ErrorMessage", "ErrorCode");

        Assert.Equal(ticketError1, ticketError2);
    }

    [Fact]
    public void ItShouldBeEqualWhenSingularWithDifferentContext()
    {
        var ticketError1 = new TicketError("ErrorMessage", "ErrorCode", new Dictionary<string, object>{{"key", "value"}});
        var ticketError2 = new TicketError("ErrorMessage", "ErrorCode", new Dictionary<string, object>{{"foo", "bar"}});

        Assert.NotEqual(ticketError1, ticketError2);
    }

    [Fact]
    public void ItShouldBeEqualWhenMultiple()
    {
        var context = new Dictionary<string, object>
        {
            ["key"] = "value"
        };
        var ticketError1 = new TicketError([
            new TicketError("ErrorMessage1", "ErrorCode1", context),
            new TicketError("ErrorMessage2", "ErrorCode2"),
            new TicketError("ErrorMessage3", "ErrorCode3")
        ]);
        var ticketError2 = new TicketError([
            new TicketError("ErrorMessage1", "ErrorCode1", context),
            new TicketError("ErrorMessage2", "ErrorCode2"),
            new TicketError("ErrorMessage3", "ErrorCode3")
        ]);

        Assert.Equal(ticketError1, ticketError2);
    }

    [Fact]
    public void ItShouldNotBeEqualWhenErrorCollectionDiffers()
    {
        var ticketError1 = new TicketError([
            new TicketError("ErrorMessage1", "ErrorCode1"),
            new TicketError("ErrorMessage2", "ErrorCode2"),
            new TicketError("ErrorMessage3", "ErrorCode3")
        ]);
        var ticketError2 = new TicketError("ErrorMessage1", "ErrorCode1");

        Assert.NotEqual(ticketError1, ticketError2);
    }
}
