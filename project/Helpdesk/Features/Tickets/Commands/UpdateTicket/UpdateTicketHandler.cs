using Helpdesk.Common;
using Helpdesk.Common.Errors;

namespace Helpdesk.Features.Tickets.Commands.UpdateTicket;

public class UpdateTicketHandler(ITicketStore store) : ICommandHandler<UpdateTicketCommand, Unit>
{
    public async Task<Unit> HandleAsync(UpdateTicketCommand command, CancellationToken cancellationToken = default)
    {
        if (!await store.UpdateAsync(command.Id, command.Title, command.Priority, cancellationToken))
        {
            throw new DomainException(TicketErrors.NotFound(command.Id));
        }

        return Unit.Value;
    }
}
