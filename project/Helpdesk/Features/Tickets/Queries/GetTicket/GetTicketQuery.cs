using Helpdesk.Common;

namespace Helpdesk.Features.Tickets.Queries.GetTicket;

public record GetTicketQuery(int Id) : IQuery<TicketDto?>;
