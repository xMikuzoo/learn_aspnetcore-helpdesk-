using Helpdesk.Common;
using Microsoft.AspNetCore.Http.HttpResults;

namespace Helpdesk.Features.Tickets.Queries.ListTickets;

public static class ListTicketsEndpoint
{
    public static void Map(RouteGroupBuilder tickets) =>
        tickets.MapGet("/", HandleAsync).WithName("ListTickets");

    private static async Task<Ok<IReadOnlyList<TicketDto>>> HandleAsync(
        IQueryHandler<ListTicketsQuery, IReadOnlyList<TicketDto>> handler,
        CancellationToken cancellationToken)
    {
        var tickets = await handler.HandleAsync(new ListTicketsQuery(), cancellationToken);
        return TypedResults.Ok(tickets);
    }
}
