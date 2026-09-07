using Helpdesk.Common;
using Helpdesk.Common.Errors;

namespace Helpdesk.Features.Tickets.Commands.UpdateTicket;

public class UpdateTicketHandler(ITicketStore store) : ICommandHandler<UpdateTicketCommand>
{
    public async Task Handle(UpdateTicketCommand command, CancellationToken cancellationToken)
    {
        if (!await store.UpdateAsync(command.Id, command.Title, command.Priority, cancellationToken))
        {
            throw new DomainException(TicketErrors.NotFound(command.Id));
        }
    }
}
