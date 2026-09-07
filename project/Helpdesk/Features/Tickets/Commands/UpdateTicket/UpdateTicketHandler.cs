using Helpdesk.Common;
using Helpdesk.Common.Errors;

namespace Helpdesk.Features.Tickets.Commands.UpdateTicket;

public class UpdateTicketHandler(ITicketStore store) : ICommandHandler<UpdateTicketCommand, Unit>
{
    public Task<Unit> HandleAsync(UpdateTicketCommand command, CancellationToken cancellationToken = default)
    {
        if (!store.Update(command.Id, command.Title, command.Priority))
        {
            throw new DomainException(TicketErrors.NotFound(command.Id));
        }

        return Task.FromResult(Unit.Value);
    }
}
