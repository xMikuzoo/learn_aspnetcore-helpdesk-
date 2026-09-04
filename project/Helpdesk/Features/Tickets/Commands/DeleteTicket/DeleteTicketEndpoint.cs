using Helpdesk.Common;
using Microsoft.AspNetCore.Http.HttpResults;

namespace Helpdesk.Features.Tickets.Commands.DeleteTicket;

public static class DeleteTicketEndpoint
{
    public static void Map(RouteGroupBuilder tickets) =>
        tickets.MapDelete("/{id:int}", HandleAsync).WithName("DeleteTicket");

    private static async Task<Results<NoContent, NotFound>> HandleAsync(
        int id,
        ICommandHandler<DeleteTicketCommand, bool> handler,
        CancellationToken cancellationToken)
    {
        var deleted = await handler.HandleAsync(new DeleteTicketCommand(id), cancellationToken);
        return deleted ? TypedResults.NoContent() : TypedResults.NotFound();
    }
}
