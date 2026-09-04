using Helpdesk.Common;

namespace Helpdesk.Features.Tickets.Queries.ListTickets;

public class ListTicketsHandler(ITicketStore store) : IQueryHandler<ListTicketsQuery, IReadOnlyList<TicketDto>>
{
    public Task<IReadOnlyList<TicketDto>> HandleAsync(ListTicketsQuery query, CancellationToken cancellationToken = default)
    {
        IReadOnlyList<TicketDto> tickets = [.. store.GetAll().Select(TicketDto.From)];
        return Task.FromResult(tickets);
    }
}
