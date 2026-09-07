using Helpdesk.Common;

namespace Helpdesk.Features.Tickets.Commands.DeleteTicket;

public record DeleteTicketCommand(int Id) : ICommand;
