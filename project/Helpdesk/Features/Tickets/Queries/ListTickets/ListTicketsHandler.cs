using Helpdesk.Common;

namespace Helpdesk.Features.Tickets.Queries.ListTickets;

public class ListTicketsHandler(ITicketStore store) : IQueryHandler<ListTicketsQuery, IReadOnlyList<TicketDto>>
{
    public async Task<IReadOnlyList<TicketDto>> Handle(ListTicketsQuery query, CancellationToken cancellationToken)
    {
        var tickets = await store.GetAllAsync(cancellationToken);

        return [.. tickets.Select(TicketDto.From)];
    }
}
