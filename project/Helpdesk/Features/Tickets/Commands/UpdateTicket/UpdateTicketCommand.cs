using Helpdesk.Common;

namespace Helpdesk.Features.Tickets.Commands.UpdateTicket;

public record UpdateTicketRequest(string Title, string Priority);

public record UpdateTicketCommand(int Id, string Title, string Priority) : ICommand;
