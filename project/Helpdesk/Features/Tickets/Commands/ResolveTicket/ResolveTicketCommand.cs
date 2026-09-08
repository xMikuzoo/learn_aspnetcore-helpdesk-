using Helpdesk.Common;

namespace Helpdesk.Features.Tickets.Commands.ResolveTicket;

public record ResolveTicketCommand(int Id) : ICommand;
