namespace TicketingService.Storage.PgSqlMarten;

using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Abstractions;
using Marten;
using Marten.Schema.Identity;

public class MartenTicketing : ITicketing
{
    private readonly IDocumentStore _store;

    public MartenTicketing(IDocumentStore store)
    {
        _store = store;
    }

    public async Task<Guid> CreateTicket(string originator, CancellationToken cancellationToken = default)
    {
        var ticketId = CombGuidIdGeneration.NewGuid();

        await using var session = _store.DirtyTrackedSession();
        session.Insert(new Ticket(ticketId, originator, TicketStatus.Created));
        await session.SaveChangesAsync(cancellationToken);

        return ticketId;
    }

    public async Task<IEnumerable<Ticket>> GetAll(CancellationToken cancellationToken = default)
    {
        await using var session = _store.QuerySession();
        return await session.Query<Ticket>().ToListAsync(cancellationToken);
    }

    public async Task<Ticket?> Get(Guid ticketId, CancellationToken cancellationToken = default)
    {
        await using var session = _store.QuerySession();
        return await session.LoadAsync<Ticket>(ticketId, cancellationToken);
    }

    private async Task ChangeStatus(Guid ticketId, TicketStatus newStatus, TicketResult? result = null, CancellationToken cancellationToken = default)
    {
        await using var session = _store.DirtyTrackedSession();
        var ticket = await session.LoadAsync<Ticket>(ticketId, cancellationToken);
        if (ticket is not null)
        {
            ticket.ChangeStatus(newStatus, result);
            session.Update(ticket);
            await session.SaveChangesAsync(cancellationToken);
        }
    }

    public Task Pending(Guid ticketId, CancellationToken cancellationToken = default) => ChangeStatus(ticketId, TicketStatus.Pending, cancellationToken: cancellationToken);

    public Task Complete(Guid ticketId, TicketResult result, CancellationToken cancellationToken = default) => ChangeStatus(ticketId, TicketStatus.Complete, result, cancellationToken);

    public async Task Delete(Guid ticketId, CancellationToken cancellationToken = default)
    {
        await using var session = _store.DirtyTrackedSession();
        session.Delete<Ticket>(ticketId);
        await session.SaveChangesAsync(cancellationToken);
    }
}
