using MediatR;
using Microsoft.AspNetCore.Http.HttpResults;

namespace Helpdesk.Features.Tickets.Commands.ResolveTicket;

public static class ResolveTicketEndpoint
{
    public static void Map(RouteGroupBuilder tickets) =>
        tickets.MapPost("/{id:int}/resolve", HandleAsync).WithName("ResolveTicket");

    private static async Task<NoContent> HandleAsync(int id, ISender sender, CancellationToken cancellationToken)
    {
        await sender.Send(new ResolveTicketCommand(id), cancellationToken);
        return TypedResults.NoContent();
    }
}
