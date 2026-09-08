namespace Helpdesk.Features.Tickets;

public record TicketDto(int Id, string Title, string Priority, string Status, int RequesterId)
{
    public static TicketDto From(Ticket ticket) =>
        new(ticket.Id, ticket.Title, ticket.Priority, ticket.Status.ToString(), ticket.RequesterId);
}
