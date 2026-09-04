using Helpdesk.Common;
using Microsoft.AspNetCore.Http.HttpResults;

namespace Helpdesk.Features.Tickets.Queries.GetTicketMetrics;

public static class GetTicketMetricsEndpoint
{
    public static void Map(RouteGroupBuilder tickets) =>
        tickets.MapGet("/metrics", HandleAsync).WithName("GetTicketMetrics");

    private static async Task<Ok<TicketMetricsDto>> HandleAsync(
        IQueryHandler<GetTicketMetricsQuery, TicketMetricsDto> handler,
        CancellationToken cancellationToken)
    {
        var metrics = await handler.HandleAsync(new GetTicketMetricsQuery(), cancellationToken);
        return TypedResults.Ok(metrics);
    }
}
