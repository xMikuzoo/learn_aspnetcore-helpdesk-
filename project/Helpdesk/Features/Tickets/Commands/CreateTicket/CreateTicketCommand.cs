using Helpdesk.Common;

namespace Helpdesk.Features.Tickets.Commands.CreateTicket;

public record CreateTicketRequest(string Title, string Priority);

public record CreateTicketCommand(string Title, string Priority) : ICommand<TicketDto>;
