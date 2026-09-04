using Helpdesk.Common;

namespace Helpdesk.Features.Tickets.Queries.GetTicketMetrics;

public record GetTicketMetricsQuery : IQuery<TicketMetricsDto>;

public record TicketMetricsDto(int Created);
