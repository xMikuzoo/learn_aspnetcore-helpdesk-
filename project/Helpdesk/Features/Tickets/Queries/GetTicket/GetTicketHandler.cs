using Helpdesk.Common;
using Helpdesk.Common.Errors;

namespace Helpdesk.Features.Tickets.Queries.GetTicket;

public class GetTicketHandler(ITicketStore store) : IQueryHandler<GetTicketQuery, TicketDto>
{
    public Task<TicketDto> HandleAsync(GetTicketQuery query, CancellationToken cancellationToken = default)
    {
        var ticket = store.GetById(query.Id) ?? throw new DomainException(TicketErrors.NotFound(query.Id));

        return Task.FromResult(TicketDto.From(ticket));
    }
}
