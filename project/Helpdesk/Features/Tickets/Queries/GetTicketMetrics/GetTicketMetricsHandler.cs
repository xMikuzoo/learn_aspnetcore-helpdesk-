using Helpdesk.Common;

namespace Helpdesk.Features.Tickets.Queries.GetTicketMetrics;

public class GetTicketMetricsHandler(TicketMetrics metrics)
    : IQueryHandler<GetTicketMetricsQuery, TicketMetricsDto>
{
    public Task<TicketMetricsDto> Handle(GetTicketMetricsQuery query, CancellationToken cancellationToken)
        => Task.FromResult(new TicketMetricsDto(metrics.Created));
}
