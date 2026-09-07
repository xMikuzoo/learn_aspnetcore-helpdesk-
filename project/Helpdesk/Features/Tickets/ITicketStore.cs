namespace Helpdesk.Features.Tickets;

public interface ITicketStore
{
    Task<IReadOnlyList<Ticket>> GetAllAsync(CancellationToken cancellationToken = default);
    Task<Ticket?> GetByIdAsync(int id, CancellationToken cancellationToken = default);
    Task<bool> TitleExistsAsync(string title, CancellationToken cancellationToken = default);
    Task<Ticket> AddAsync(string title, string priority, CancellationToken cancellationToken = default);
    Task<bool> UpdateAsync(int id, string title, string priority, CancellationToken cancellationToken = default);
    Task<bool> DeleteAsync(int id, CancellationToken cancellationToken = default);
}
