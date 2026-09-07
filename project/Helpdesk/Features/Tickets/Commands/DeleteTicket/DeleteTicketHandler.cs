using Helpdesk.Common;
using Helpdesk.Common.Errors;

namespace Helpdesk.Features.Tickets.Commands.DeleteTicket;

public class DeleteTicketHandler(ITicketStore store) : ICommandHandler<DeleteTicketCommand, Unit>
{
    public async Task<Unit> HandleAsync(DeleteTicketCommand command, CancellationToken cancellationToken = default)
    {
        if (!await store.DeleteAsync(command.Id, cancellationToken))
        {
            throw new DomainException(TicketErrors.NotFound(command.Id));
        }

        return Unit.Value;
    }
}
