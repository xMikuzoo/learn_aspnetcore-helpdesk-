namespace Helpdesk.Features.Tickets;

public class InMemoryTicketStore : ITicketStore
{
    private readonly Lock _gate = new();
    private readonly List<Ticket> _tickets = [
        new Ticket(1, "Pierwsze zgłoszenie", "normal"),
        new Ticket(2, "Drugie zgłoszenie!", "high"),
    ];

    private int GenerateId() =>
        _tickets.Count == 0 ? 1 : _tickets.Max(t => t.Id) + 1;

    public IReadOnlyList<Ticket> GetAll()
    {
        lock (_gate)
        {
            return _tickets.ToList();
        }
    }

    public Ticket? GetById(int id)
    {
        lock (_gate)
        {
            return _tickets.FirstOrDefault(t => t.Id == id);
        }
    }

    public Ticket Add(string title, string priority)
    {
        lock (_gate)
        {
            var ticket = new Ticket(GenerateId(), title, priority);
            _tickets.Add(ticket);
            return ticket;
        }
    }

    public bool Update(int id, string title, string priority)
    {
        lock (_gate)
        {
            var index = _tickets.FindIndex(t => t.Id == id);
            if (index < 0)
            {
                return false;
            }
            _tickets[index] = _tickets[index] with { Title = title, Priority = priority };
            return true;
        }
    }

    public bool Delete(int id)
    {
        lock (_gate)
        {
            return _tickets.RemoveAll(t => t.Id == id) > 0;
        }
    }
}
