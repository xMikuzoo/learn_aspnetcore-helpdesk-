using Helpdesk.Common;
using Microsoft.AspNetCore.Http.HttpResults;

namespace Helpdesk.Features.Tickets.Queries.GetTicket;

public static class GetTicketEndpoint
{
    public static void Map(RouteGroupBuilder tickets) =>
        tickets.MapGet("/{id:int}", HandleAsync).WithName("GetTicket");

    private static async Task<Results<Ok<TicketDto>, NotFound>> HandleAsync(
        int id,
        IQueryHandler<GetTicketQuery, TicketDto?> handler,
        CancellationToken cancellationToken)
    {
        var ticket = await handler.HandleAsync(new GetTicketQuery(id), cancellationToken);
        return ticket is { } found ? TypedResults.Ok(found) : TypedResults.NotFound();
    }
}
