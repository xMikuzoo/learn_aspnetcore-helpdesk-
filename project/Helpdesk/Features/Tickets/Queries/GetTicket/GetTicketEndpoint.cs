using Helpdesk.Common;
using Microsoft.AspNetCore.Http.HttpResults;

namespace Helpdesk.Features.Tickets.Queries.GetTicket;

public static class GetTicketEndpoint
{
    public static void Map(RouteGroupBuilder tickets) =>
        tickets.MapGet("/{id:int}", HandleAsync).WithName("GetTicket");

    private static async Task<Ok<TicketDto>> HandleAsync(
        int id,
        ISender sender,
        CancellationToken cancellationToken)
    {
        var ticket = await sender.Send(new GetTicketQuery(id), cancellationToken);

        return TypedResults.Ok(ticket);
    }
}
