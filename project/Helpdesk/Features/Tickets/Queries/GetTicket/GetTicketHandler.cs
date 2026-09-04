using Helpdesk.Common;

namespace Helpdesk.Features.Tickets.Queries.GetTicket;

public class GetTicketHandler(ITicketStore store) : IQueryHandler<GetTicketQuery, TicketDto?>
{
    public Task<TicketDto?> HandleAsync(GetTicketQuery query, CancellationToken cancellationToken = default)
    {
        var ticket = store.GetById(query.Id);
        return Task.FromResult(ticket is null ? null : TicketDto.From(ticket));
    }
}
