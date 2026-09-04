public interface ITicketStore
{
    IReadOnlyList<Ticket> GetAll();
    Ticket? GetById(int id);
    Ticket Add(string title);
}
