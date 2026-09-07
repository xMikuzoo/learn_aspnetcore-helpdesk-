using Helpdesk.Common;
using Microsoft.AspNetCore.Http.HttpResults;

namespace Helpdesk.Features.Tickets.Commands.UpdateTicket;

public static class UpdateTicketEndpoint
{
    public static void Map(RouteGroupBuilder tickets) =>
        tickets.MapPut("/{id:int}", HandleAsync).WithName("UpdateTicket");

    private static async Task<NoContent> HandleAsync(
        int id,
        UpdateTicketRequest request,
        ISender sender,
        CancellationToken cancellationToken)
    {
        await sender.Send(new UpdateTicketCommand(id, request.Title, request.Priority), cancellationToken);

        return TypedResults.NoContent();
    }
}
