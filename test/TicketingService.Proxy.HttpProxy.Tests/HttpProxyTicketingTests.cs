namespace TicketingService.Proxy.HttpProxy.Tests;

using System;
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
        var result = await ticketing.CreateTicket("originator");
        Assert.Equal(expectedResult, result);
    }

    [Fact]
    public async Task GetAll()
    {
        var expectedResult = new []
        {
            new Ticket(Guid.NewGuid(), "originator", TicketStatus.Created),
            new Ticket(Guid.NewGuid(), "originator", TicketStatus.Complete, new TicketResult("Complete"))
        };
        var httpClient = MockHttpClient(expectedResult);

        var ticketing = new HttpProxyTicketing(httpClient: httpClient);
        var result = await ticketing.GetAll();
        Assert.Equal(expectedResult, result);
    }

    [Fact]
    public async Task Get()
    {
        var expectedResult = new Ticket(Guid.NewGuid(), "originator", TicketStatus.Created);
        var httpClient = MockHttpClient(expectedResult);

        var ticketing = new HttpProxyTicketing(httpClient: httpClient);
        var result = await ticketing.Get(expectedResult.TicketId);
        Assert.Equal(expectedResult, result);
    }
}
