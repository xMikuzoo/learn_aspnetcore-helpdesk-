using Helpdesk.Common;
using Helpdesk.Common.Errors;

namespace Helpdesk.Features.Tickets.Commands.DeleteTicket;

public class DeleteTicketHandler(ITicketStore store) : ICommandHandler<DeleteTicketCommand>
{
    public async Task Handle(DeleteTicketCommand command, CancellationToken cancellationToken)
    {
        if (!await store.DeleteAsync(command.Id, cancellationToken))
        {
            throw new DomainException(TicketErrors.NotFound(command.Id));
        }
    }
}
