using Helpdesk.Common;
using Helpdesk.Common.Errors;

namespace Helpdesk.Features.Tickets.Commands.DeleteTicket;

public class DeleteTicketHandler(ITicketStore store) : ICommandHandler<DeleteTicketCommand, Unit>
{
    public Task<Unit> HandleAsync(DeleteTicketCommand command, CancellationToken cancellationToken = default)
    {
        if (!store.Delete(command.Id))
        {
            throw new DomainException(TicketErrors.NotFound(command.Id));
        }

        return Task.FromResult(Unit.Value);
    }
}
