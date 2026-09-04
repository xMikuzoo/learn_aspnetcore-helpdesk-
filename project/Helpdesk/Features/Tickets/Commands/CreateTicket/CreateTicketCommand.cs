using Helpdesk.Common;

namespace Helpdesk.Features.Tickets.Commands.CreateTicket;

public record CreateTicketRequest(string Title);

public record CreateTicketCommand(string Title) : ICommand<TicketDto>;
