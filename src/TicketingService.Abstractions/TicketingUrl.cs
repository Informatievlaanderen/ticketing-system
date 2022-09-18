namespace TicketingService.Abstractions;

using System;

public class TicketingUrl : ITicketingUrl
{
    private readonly Uri _baseUrl;

    public TicketingUrl(string baseUrl) : this(new Uri(baseUrl))
    { }

    public TicketingUrl(Uri baseUrl)
    {
        _baseUrl = baseUrl;
    }

    public Uri For(Guid ticketId)
        => Combine(_baseUrl, ticketId.ToString("D"));

    private static Uri Combine(Uri uri, string partOfPath)
        => new Uri($"{uri.ToString().TrimEnd('/')}/{partOfPath.TrimStart('/')}");
}
