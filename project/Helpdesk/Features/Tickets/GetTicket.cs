using Helpdesk.Common;

namespace Helpdesk.Features.Tickets;

public record GetTicketQuery(int Id);

public class GetTicketHandler(ITicketStore store) : IQueryHandler<GetTicketQuery, Ticket?>
{
    public Task<Ticket?> HandleAsync(GetTicketQuery query, CancellationToken cancellationToken = default)
    {
        var ticket = store.GetById(query.Id);
        return Task.FromResult(ticket);
    }
}
