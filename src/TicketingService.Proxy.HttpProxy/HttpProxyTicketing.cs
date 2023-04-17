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

    public HttpProxyTicketing(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<Guid> CreateTicket(IDictionary<string, string>? metadata = null, CancellationToken cancellationToken = default)
    {
        var response = await _httpClient.PostAsync("/tickets/create", JsonContent.Create(metadata ?? new Dictionary<string, string>()), cancellationToken);

        response.EnsureSuccessStatusCode();

        return await JsonSerializer.DeserializeAsync<Guid>(await response.Content.ReadAsStreamAsync(cancellationToken), cancellationToken: cancellationToken);
    }

    public async Task<IEnumerable<Ticket>> GetAll(CancellationToken cancellationToken = default)
        => await _httpClient.GetFromJsonAsync<IEnumerable<Ticket>>("/tickets", cancellationToken) ?? Enumerable.Empty<Ticket>();

    public async Task<Ticket?> Get(Guid ticketId, CancellationToken cancellationToken = default)
        => await _httpClient.GetFromJsonAsync<Ticket>($"/tickets/{ticketId:D}", cancellationToken: cancellationToken);

    public async Task Pending(Guid ticketId, CancellationToken cancellationToken = default)
        => await _httpClient.PutAsync($"/tickets/{ticketId}/pending", new ReadOnlyMemoryContent(null), cancellationToken);

    public async Task Complete(Guid ticketId, TicketResult result, CancellationToken cancellationToken = default)
        => await _httpClient.PutAsJsonAsync($"/tickets/{ticketId}/complete", result, cancellationToken: cancellationToken);

    public async Task Error(Guid ticketId, TicketError error, CancellationToken cancellationToken = default)
        => await _httpClient.PutAsJsonAsync($"/tickets/{ticketId}/error", error, cancellationToken: cancellationToken);

    public async Task Delete(Guid ticketId, CancellationToken cancellationToken = default)
        => await _httpClient.DeleteAsync($"/tickets/{ticketId}", cancellationToken);
}
