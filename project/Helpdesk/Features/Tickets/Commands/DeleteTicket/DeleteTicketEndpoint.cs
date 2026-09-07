using MediatR;
using Microsoft.AspNetCore.Http.HttpResults;

namespace Helpdesk.Features.Tickets.Commands.DeleteTicket;

public static class DeleteTicketEndpoint
{
    public static void Map(RouteGroupBuilder tickets) =>
        tickets.MapDelete("/{id:int}", HandleAsync).WithName("DeleteTicket");

    private static async Task<NoContent> HandleAsync(
        int id,
        ISender sender,
        CancellationToken cancellationToken)
    {
        await sender.Send(new DeleteTicketCommand(id), cancellationToken);

        return TypedResults.NoContent();
    }
}
