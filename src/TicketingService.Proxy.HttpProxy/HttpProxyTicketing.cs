namespace TicketingService.Proxy.HttpProxy;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using Abstractions;

public class HttpProxyTicketing : ITicketing
{
    private readonly HttpClient _httpClient;

    public HttpProxyTicketing(string? baseAddress = null, HttpClient? httpClient = null)
    {
        _httpClient = httpClient ?? new HttpClient();
        if (baseAddress is not null && baseAddress.EndsWith("/"))
        {
            _httpClient.BaseAddress = new Uri(baseAddress.TrimEnd('/'));
        }
    }

    public async Task<Guid> CreateTicket(IDictionary<string, string>? metadata = null, CancellationToken cancellationToken = default)
    {
        var ticketId = Guid.Empty;
        var response = await _httpClient.PostAsync("/tickets/create", JsonContent.Create(metadata ?? new Dictionary<string, string>()), cancellationToken);
        if (response.IsSuccessStatusCode)
        {
            ticketId = await JsonSerializer.DeserializeAsync<Guid>(await response.Content.ReadAsStreamAsync(cancellationToken), cancellationToken: cancellationToken);
        }

        return ticketId;
    }

    public async Task<IEnumerable<Ticket>> GetAll(CancellationToken cancellationToken = default) => await _httpClient.GetFromJsonAsync<IEnumerable<Ticket>>("/tickets", cancellationToken)
        ?? Enumerable.Empty<Ticket>();

    public async Task<Ticket?> Get(Guid ticketId, CancellationToken cancellationToken = default) => await _httpClient.GetFromJsonAsync<Ticket>($"/tickets/{ticketId:D}", cancellationToken: cancellationToken);

    public async Task Pending(Guid ticketId, CancellationToken cancellationToken = default) => await _httpClient.PutAsync($"/tickets/{ticketId}/pending", new ReadOnlyMemoryContent(null), cancellationToken);

    public async Task Complete(Guid ticketId, TicketResult result, CancellationToken cancellationToken = default) => await _httpClient.PutAsJsonAsync($"/tickets/{ticketId}/complete", result, cancellationToken: cancellationToken);

    public async Task Delete(Guid ticketId, CancellationToken cancellationToken = default) => await _httpClient.DeleteAsync($"/tickets/{ticketId}", cancellationToken);
}
