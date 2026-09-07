using Helpdesk.Common;
using Helpdesk.Common.Errors;

namespace Helpdesk.Features.Tickets.Commands.CreateTicket;

public class CreateTicketHandler(ITicketStore store, TicketMetrics metrics, ICurrentUser user)
    : ICommandHandler<CreateTicketCommand, TicketDto>
{
    public Task<TicketDto> HandleAsync(CreateTicketCommand command, CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(command.Title))
        {
            throw new DomainException(TicketErrors.EmptyTitle);
        }

        var ticket = store.Add(command.Title);
        metrics.RecordCreated(user);

        return Task.FromResult(TicketDto.From(ticket));
    }
}
