using Helpdesk.Common;
using Microsoft.AspNetCore.Http.HttpResults;

namespace Helpdesk.Features.Tickets.Commands.UpdateTicket;

public static class UpdateTicketEndpoint
{
    public static void Map(RouteGroupBuilder tickets) =>
        tickets.MapPut("/{id:int}", HandleAsync).WithName("UpdateTicket");

    private static async Task<Results<NoContent, NotFound>> HandleAsync(
        int id,
        UpdateTicketRequest request,
        ICommandHandler<UpdateTicketCommand, bool> handler,
        CancellationToken cancellationToken)
    {
        var updated = await handler.HandleAsync(new UpdateTicketCommand(id, request.Title), cancellationToken);
        return updated ? TypedResults.NotFound() : TypedResults.NotFound();
    }
}
