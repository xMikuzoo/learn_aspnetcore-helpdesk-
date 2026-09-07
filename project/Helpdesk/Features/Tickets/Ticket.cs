namespace Helpdesk.Features.Tickets;

public class Ticket
{
    private Ticket(string title, string priority)
    {
        Title = title;
        Priority = priority;
    }

    public int Id { get; private set; }
    public string Title { get; private set; }
    public string Priority { get; private set; }

    public static Ticket Create(string title, string priority) => new(title, priority);

    public void Rename(string title) => Title = title;

    public void ChangePriority(string priority) => Priority = priority;
}
