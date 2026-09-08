using Helpdesk.Common.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Helpdesk.Features.Tickets;

public class EfTicketStore(HelpdeskDbContext db) : ITicketStore
{
    public async Task<IReadOnlyList<Ticket>> GetAllAsync(CancellationToken cancellationToken = default) =>
        await db.Tickets.AsNoTracking().OrderBy(t => t.Id).ToListAsync(cancellationToken);

    public Task<Ticket?> GetByIdAsync(int id, CancellationToken cancellationToken = default) =>
        db.Tickets.AsNoTracking().FirstOrDefaultAsync(t => t.Id == id, cancellationToken);

    public Task<int?> FindRequesterIdAsync(string login, CancellationToken cancellationToken = default) =>
        db.Requesters
            .Where(r => r.Login == login)
            .Select(r => (int?)r.Id)
            .FirstOrDefaultAsync(cancellationToken);

    public Task<bool> UnresolvedTitleExistsAsync(int requesterId, string title, CancellationToken cancellationToken = default) =>
        db.Tickets.AnyAsync(
            t => t.RequesterId == requesterId && t.Title == title && t.Status != TicketStatus.Resolved,
            cancellationToken);

    public async Task<Ticket> AddAsync(string title, string priority, int requesterId, CancellationToken cancellationToken = default)
    {
        var ticket = Ticket.Create(title, priority, requesterId);

        db.Tickets.Add(ticket);
        await db.SaveChangesAsync(cancellationToken);

        return ticket;
    }

    public async Task<bool> UpdateAsync(int id, string title, string priority, CancellationToken cancellationToken = default)
    {
        var ticket = await db.Tickets.FirstOrDefaultAsync(t => t.Id == id, cancellationToken);
        if (ticket is null)
        {
            return false;
        }

        ticket.Rename(title);
        ticket.ChangePriority(priority);
        await db.SaveChangesAsync(cancellationToken);

        return true;
    }

    public async Task<bool> ResolveAsync(int id, CancellationToken cancellationToken = default)
    {
        var ticket = await db.Tickets.FirstOrDefaultAsync(t => t.Id == id, cancellationToken);
        if (ticket is null)
        {
            return false;
        }

        ticket.Resolve();
        await db.SaveChangesAsync(cancellationToken);

        return true;
    }

    public async Task<bool> DeleteAsync(int id, CancellationToken cancellationToken = default)
    {
        var ticket = await db.Tickets.FirstOrDefaultAsync(t => t.Id == id, cancellationToken);
        if (ticket is null)
        {
            return false;
        }

        db.Tickets.Remove(ticket);
        await db.SaveChangesAsync(cancellationToken);

        return true;
    }
}
