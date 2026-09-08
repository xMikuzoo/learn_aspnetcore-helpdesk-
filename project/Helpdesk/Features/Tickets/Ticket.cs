using Helpdesk.Features.Requesters;

namespace Helpdesk.Features.Tickets;

public class Ticket
{
    private Ticket(string title, string priority, int requesterId)
    {
        Title = title;
        Priority = priority;
        RequesterId = requesterId;
    }

    public int Id { get; private set; }
    public string Title { get; private set; }
    public string Priority { get; private set; }
    public TicketStatus Status { get; private set; }
    public int RequesterId { get; private set; }
    public Requester? Requester { get; private set; }

    public static Ticket Create(string title, string priority, int requesterId) =>
        new(title, priority, requesterId);

    public void Rename(string title) => Title = title;

    public void ChangePriority(string priority) => Priority = priority;

    public void Resolve() => Status = TicketStatus.Resolved;
}
