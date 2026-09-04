using Helpdesk.Common;

namespace Helpdesk.Features.Tickets.Commands.UpdateTicket;

public class UpdateTicketHandler(ITicketStore store) : ICommandHandler<UpdateTicketCommand, bool>
{
    public Task<bool> HandleAsync(UpdateTicketCommand command, CancellationToken cancellationToken = default)
        => Task.FromResult(store.Update(command.Id, command.Title));
}
