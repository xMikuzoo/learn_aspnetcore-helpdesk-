using Helpdesk.Common;

namespace Helpdesk.Features.Tickets.Commands.CreateTicket;

public class CreateTicketHandler(ITicketStore store, TicketMetrics metrics, ICurrentUser user)
    : ICommandHandler<CreateTicketCommand, TicketDto>
{
    public Task<TicketDto> HandleAsync(CreateTicketCommand command, CancellationToken cancellationToken = default)
    {
        var ticket = store.Add(command.Title);
        metrics.RecordCreated(user);
        return Task.FromResult(TicketDto.From(ticket));
    }
}
