using Helpdesk.Common;

namespace Helpdesk.Features.Tickets.Queries.GetTicketMetrics;

public class GetTicketMetricsHandler(TicketMetrics metrics)
    : IQueryHandler<GetTicketMetricsQuery, TicketMetricsDto>
{
    public Task<TicketMetricsDto> HandleAsync(GetTicketMetricsQuery query, CancellationToken cancellationToken = default)
        => Task.FromResult(new TicketMetricsDto(metrics.Created));
}
