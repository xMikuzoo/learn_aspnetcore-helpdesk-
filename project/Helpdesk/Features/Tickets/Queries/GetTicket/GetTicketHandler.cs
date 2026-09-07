using Helpdesk.Common;
using Helpdesk.Common.Errors;

namespace Helpdesk.Features.Tickets.Queries.GetTicket;

public class GetTicketHandler(ITicketStore store) : IQueryHandler<GetTicketQuery, TicketDto>
{
    public async Task<TicketDto> Handle(GetTicketQuery query, CancellationToken cancellationToken)
    {
        var ticket = await store.GetByIdAsync(query.Id, cancellationToken)
            ?? throw new DomainException(TicketErrors.NotFound(query.Id));

        return TicketDto.From(ticket);
    }
}
