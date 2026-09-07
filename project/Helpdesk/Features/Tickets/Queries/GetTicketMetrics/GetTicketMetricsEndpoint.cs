using MediatR;
using Microsoft.AspNetCore.Http.HttpResults;

namespace Helpdesk.Features.Tickets.Queries.GetTicketMetrics;

public static class GetTicketMetricsEndpoint
{
    public static void Map(RouteGroupBuilder tickets) =>
        tickets.MapGet("/metrics", HandleAsync).WithName("GetTicketMetrics");

    private static async Task<Ok<TicketMetricsDto>> HandleAsync(
        ISender sender,
        CancellationToken cancellationToken)
    {
        var metrics = await sender.Send(new GetTicketMetricsQuery(), cancellationToken);
        return TypedResults.Ok(metrics);
    }
}
