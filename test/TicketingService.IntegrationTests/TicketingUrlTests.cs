namespace TicketingService.IntegrationTests;

using System;
using Abstractions;
using FluentAssertions;
using Xunit;

public class TicketingUrlTests
{
    [Theory]
    [InlineData("https://api.basisregisters.vlaanderen.be/ticketing")]
    [InlineData("https://api.basisregisters.vlaanderen.be/ticketing/")]
    public void GivenBaseUrl_ThenIdIsAppended(string baseUrl)
    {
        var ticketId = Guid.NewGuid();

        var sut = new TicketingUrl(baseUrl);

        sut.For(ticketId).Should().Be(new Uri($"{baseUrl.TrimEnd('/')}/{ticketId:D}"));
    }
}
