using Helpdesk.Common;
using Microsoft.AspNetCore.Http.HttpResults;

namespace Helpdesk.Features.Tickets.Commands.DeleteTicket;

public static class DeleteTicketEndpoint
{
    public static void Map(RouteGroupBuilder tickets) =>
        tickets.MapDelete("/{id:int}", HandleAsync).WithName("DeleteTicket");

    private static async Task<Results<NoContent, NotFound>> HandleAsync(
        int id,
        ISender sender,
        CancellationToken cancellationToken)
    {
        var deleted = await sender.Send(new DeleteTicketCommand(id), cancellationToken);
        return deleted ? TypedResults.NoContent() : TypedResults.NotFound();
    }
}
