namespace TicketingService.Proxy.HttpProxy.Tests;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading;
using System.Threading.Tasks;
using Abstractions;
using Moq;
using Moq.Protected;
using Xunit;

public class HttpProxyTicketingTests
{
    private static HttpClient MockHttpClient(object? value)
    {
        var httpMessageHandlerMock = new Mock<HttpMessageHandler>();
        var httpResponseMessage = new HttpResponseMessage { Content = JsonContent.Create(value) };

        httpMessageHandlerMock
            .Protected()
            .Setup<Task<HttpResponseMessage>>(nameof(HttpClient.SendAsync), ItExpr.IsAny<HttpRequestMessage>(), ItExpr.IsAny<CancellationToken>())
            .ReturnsAsync(httpResponseMessage);

        // create the HttpClient
        var httpClient = new HttpClient(httpMessageHandlerMock.Object) { BaseAddress = new Uri("http://localhost") };
        return httpClient;
    }

    [Fact]
    public async Task Create()
    {
        var expectedResult = Guid.NewGuid();
        var httpClient = MockHttpClient(expectedResult);

        var ticketing = new HttpProxyTicketing(httpClient: httpClient);
        var result = await ticketing.CreateTicket();
        Assert.Equal(expectedResult, result);
    }

    [Fact]
    public async Task GetAll()
    {
        var expectedResult = new[]
        {
            new Ticket(Guid.NewGuid(), TicketStatus.Created, new Dictionary<string, string> { { "originator", "ticketing" } }),
            new Ticket(Guid.NewGuid(), TicketStatus.Complete, new Dictionary<string, string>(), new TicketResult("Complete"))
        };
        var httpClient = MockHttpClient(expectedResult);

        var ticketing = new HttpProxyTicketing(httpClient: httpClient);
        var result = (await ticketing.GetAll()).ToList();

        Assert.Equal(expectedResult.Length, result.Count);
        foreach (var expectedTicket in expectedResult)
        {
            var ticket = result.Single(x => x.TicketId == expectedTicket.TicketId);
            AssertTicket(expectedTicket, ticket);
        }
    }

    [Fact]
    public async Task Get()
    {
        var expectedResult = new Ticket(
            Guid.NewGuid(),
            TicketStatus.Created,
            new Dictionary<string, string>
            {
                { "originator", "ticketing" }
            });
        var httpClient = MockHttpClient(expectedResult);

        var ticketing = new HttpProxyTicketing(httpClient: httpClient);
        var result = await ticketing.Get(expectedResult.TicketId);

        AssertTicket(expectedResult, result!);
    }

    private static void AssertTicket(Ticket expectedResult, Ticket result)
    {
        Assert.Equal(expectedResult.TicketId, result.TicketId);
        Assert.Equal(expectedResult.Status, result.Status);
        Assert.Equal(expectedResult.Metadata, result.Metadata);
        Assert.Equal(expectedResult.Result, result.Result);
    }
}
