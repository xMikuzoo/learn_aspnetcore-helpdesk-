using Helpdesk.Common;
using Microsoft.AspNetCore.Http.HttpResults;

namespace Helpdesk.Features.Tickets.Commands.CreateTicket;

public static class CreateTicketEndpoint
{
    public static void Map(RouteGroupBuilder tickets) =>
        tickets.MapPost("/", HandleAsync).WithName("CreateTicket");

    private static async Task<Created<TicketDto>> HandleAsync(
        CreateTicketRequest request,
        ISender sender,
        CancellationToken cancellationToken)
    {
        var ticket = await sender.Send(new CreateTicketCommand(request.Title), cancellationToken);
        return TypedResults.Created($"/tickets/{ticket.Id}", ticket);
    }
}
