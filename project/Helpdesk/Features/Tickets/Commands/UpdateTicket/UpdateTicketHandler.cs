using Helpdesk.Common;
using Helpdesk.Common.Errors;

namespace Helpdesk.Features.Tickets.Commands.UpdateTicket;

public class UpdateTicketHandler(ITicketStore store) : ICommandHandler<UpdateTicketCommand, Unit>
{
    public Task<Unit> HandleAsync(UpdateTicketCommand command, CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(command.Title))
        {
            throw new DomainException(TicketErrors.EmptyTitle);
        }

        if (!store.Update(command.Id, command.Title))
        {
            throw new DomainException(TicketErrors.NotFound(command.Id));
        }

        return Task.FromResult(Unit.Value);
    }
}
