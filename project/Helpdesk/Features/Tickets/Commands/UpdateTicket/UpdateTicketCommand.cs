using Helpdesk.Common;

namespace Helpdesk.Features.Tickets.Commands.UpdateTicket;

public record UpdateTicketRequest(string Title);

public record UpdateTicketCommand(int Id, string Title) : ICommand<bool>;
