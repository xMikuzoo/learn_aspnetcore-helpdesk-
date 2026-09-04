using Helpdesk.Common;

namespace Helpdesk.Features.Tickets.Commands.DeleteTicket;

public class DeleteTicketHandler(ITicketStore store) : ICommandHandler<DeleteTicketCommand, bool>
{
    public Task<bool> HandleAsync(DeleteTicketCommand command, CancellationToken cancellationToken = default)
        => Task.FromResult(store.Delete(command.Id));
}
