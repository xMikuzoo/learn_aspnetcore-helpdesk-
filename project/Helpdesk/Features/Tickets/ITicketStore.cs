namespace Helpdesk.Features.Tickets;

public interface ITicketStore
{
    Task<IReadOnlyList<Ticket>> GetAllAsync(CancellationToken cancellationToken = default);
    Task<Ticket?> GetByIdAsync(int id, CancellationToken cancellationToken = default);
    Task<int?> FindRequesterIdAsync(string login, CancellationToken cancellationToken = default);
    Task<bool> UnresolvedTitleExistsAsync(int requesterId, string title, CancellationToken cancellationToken = default);
    Task<Ticket> AddAsync(string title, string priority, int requesterId, CancellationToken cancellationToken = default);
    Task<bool> UpdateAsync(int id, string title, string priority, CancellationToken cancellationToken = default);
    Task<bool> ResolveAsync(int id, CancellationToken cancellationToken = default);
    Task<bool> DeleteAsync(int id, CancellationToken cancellationToken = default);
}
