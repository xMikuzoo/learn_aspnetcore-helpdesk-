namespace Helpdesk.Features.Tickets;

public interface ITicketStore
{
    IReadOnlyList<Ticket> GetAll();
    Ticket? GetById(int id);
    Ticket Add(string title);
    bool Update(int id, string title);
    bool Delete(int id);
}
