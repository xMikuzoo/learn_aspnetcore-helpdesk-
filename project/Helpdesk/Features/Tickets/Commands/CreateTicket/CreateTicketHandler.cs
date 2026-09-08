using Helpdesk.Common;
using Helpdesk.Common.Errors;

namespace Helpdesk.Features.Tickets.Commands.CreateTicket;

public class CreateTicketHandler(ITicketStore store, TicketMetrics metrics, ICurrentUser user)
    : ICommandHandler<CreateTicketCommand, TicketDto>
{
    public async Task<TicketDto> Handle(CreateTicketCommand command, CancellationToken cancellationToken)
    {
        var requesterId = await store.FindRequesterIdAsync(user.Name, cancellationToken)
            ?? throw new DomainException(TicketErrors.UnknownRequester(user.Name));

        if (await store.UnresolvedTitleExistsAsync(requesterId, command.Title, cancellationToken))
        {
            throw new DomainException(TicketErrors.DuplicateUnresolvedTitle(command.Title));
        }

        var ticket = await store.AddAsync(command.Title, command.Priority, requesterId, cancellationToken);
        metrics.RecordCreated(user);

        return TicketDto.From(ticket);
    }
}
