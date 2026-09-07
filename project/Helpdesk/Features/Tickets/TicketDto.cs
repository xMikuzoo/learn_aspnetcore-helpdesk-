namespace Helpdesk.Features.Tickets;

public record TicketDto(int Id, string Title, string Priority)
{
    public static TicketDto From(Ticket ticket) => new(ticket.Id, ticket.Title, ticket.Priority);
}
