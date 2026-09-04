using Helpdesk.Common;

namespace Helpdesk.Features.Tickets;

public record CreateTicketCommand(string Title);

public class CreateTicketHandler(ITicketStore store, TicketMetrics metrics, ICurrentUser user) : ICommandHandler<CreateTicketCommand, Ticket>
{
    public Task<Ticket> HandleAsync(CreateTicketCommand command, CancellationToken cancellationToken = default)
    {
        var ticket = store.Add(command.Title);
        metrics.RecordCreated(user);
        return Task.FromResult(ticket);
    }
}
