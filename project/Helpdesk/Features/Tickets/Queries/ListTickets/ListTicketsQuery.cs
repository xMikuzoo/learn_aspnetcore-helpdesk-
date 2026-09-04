using Helpdesk.Common;

namespace Helpdesk.Features.Tickets.Queries.ListTickets;

public record ListTicketsQuery : IQuery<IReadOnlyList<TicketDto>>;
