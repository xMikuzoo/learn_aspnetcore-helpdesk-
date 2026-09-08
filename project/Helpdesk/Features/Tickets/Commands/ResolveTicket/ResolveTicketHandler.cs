using Helpdesk.Common;
using Helpdesk.Common.Errors;

namespace Helpdesk.Features.Tickets.Commands.ResolveTicket;

public class ResolveTicketHandler(ITicketStore store) : ICommandHandler<ResolveTicketCommand>
{
    public async Task Handle(ResolveTicketCommand command, CancellationToken cancellationToken)
    {
        if (!await store.ResolveAsync(command.Id, cancellationToken))
        {
            throw new DomainException(TicketErrors.NotFound(command.Id));
        }
    }
}
