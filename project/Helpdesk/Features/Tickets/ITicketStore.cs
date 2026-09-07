namespace Helpdesk.Features.Tickets;

public interface ITicketStore
{
    IReadOnlyList<Ticket> GetAll();
    Ticket? GetById(int id);
    Ticket Add(string title, string priority);
    bool Update(int id, string title, string priority);
    bool Delete(int id);
}
