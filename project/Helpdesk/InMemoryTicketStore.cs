public class InMemoryTicketStore : ITicketStore
{
    private readonly List<Ticket> _tickets = [
        new  Ticket(1,"Pierwsze zgłoszenie"),
        new  Ticket(2,"Drugie zgłoszenie!"),
    ];

    public IReadOnlyList<Ticket> GetAll() => _tickets;

    public Ticket? GetById(int id) => _tickets.FirstOrDefault(t => t.Id == id);

    public Ticket Add(string title)
    {
        var ticket = new Ticket(_tickets.Count + 1, title);
        _tickets.Add(ticket);
        return ticket;
    }
}
