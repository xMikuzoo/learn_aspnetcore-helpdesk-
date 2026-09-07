using Helpdesk.Common;
using Helpdesk.Common.Errors;

namespace Helpdesk.Features.Tickets.Commands.CreateTicket;

public class CreateTicketHandler(ITicketStore store, TicketMetrics metrics, ICurrentUser user)
    : ICommandHandler<CreateTicketCommand, TicketDto>
{
    public async Task<TicketDto> Handle(CreateTicketCommand command, CancellationToken cancellationToken)
    {
        if (await store.TitleExistsAsync(command.Title, cancellationToken))
        {
            throw new DomainException(TicketErrors.DuplicateTitle(command.Title));
        }

        var ticket = await store.AddAsync(command.Title, command.Priority, cancellationToken);
        metrics.RecordCreated(user);

        return TicketDto.From(ticket);
    }
}
