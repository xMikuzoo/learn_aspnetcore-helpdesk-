using Helpdesk.Common;
using Microsoft.AspNetCore.Http.HttpResults;

namespace Helpdesk.Features.Tickets;

public static class TicketsFeature
{
    public static IServiceCollection AddTickets(this IServiceCollection services)
    {
        services.AddSingleton<ITicketStore, InMemoryTicketStore>();
        services.AddSingleton<TicketMetrics>();

        services.AddScoped<ICommandHandler<CreateTicketCommand, Ticket>, CreateTicketHandler>();
        services.AddScoped<IQueryHandler<GetTicketQuery, Ticket?>, GetTicketHandler>();

        return services;
    }

    public static RouteGroupBuilder MapTickets(this IEndpointRouteBuilder app)
    {
        var tickets = app.MapGroup("/tickets");

        tickets.MapGet("/", (ITicketStore store) => store.GetAll());

        tickets.MapGet("/{id:int}",
            async Task<Results<Ok<Ticket>, NotFound>> (
                int id,
                IQueryHandler<GetTicketQuery, Ticket?> handler,
                CancellationToken cancellationToken) =>
            {
                var ticket = await handler.HandleAsync(new GetTicketQuery(id), cancellationToken);
                if (ticket is { } found)
                {
                    return TypedResults.Ok(found);
                }
                return TypedResults.NotFound();
            });

        tickets.MapPost("/",
            async (CreateTicketRequest request,
                ICommandHandler<CreateTicketCommand, Ticket> handler,
                CancellationToken cancellationToken) =>
            {
                var ticket = await handler.HandleAsync(new CreateTicketCommand(request.Title), cancellationToken);
                return TypedResults.Created($"/tickets/{ticket.Id}", ticket);
            });

        tickets.MapPut("/{id:int}",
            Results<NoContent, NotFound> (int id, UpdateTicketRequest request, ITicketStore store) =>
            {
                if (store.Update(id, request.Title))
                {
                    return TypedResults.NoContent();
                }
                return TypedResults.NotFound();
            });

        tickets.MapDelete("/{id:int}",
            Results<NoContent, NotFound> (int id, ITicketStore store) =>
            {
                if (store.Delete(id))
                {
                    return TypedResults.NoContent();
                }
                return TypedResults.NotFound();
            });

        tickets.MapGet("/metrics", (TicketMetrics metrics) => new { created = metrics.Created });

        return tickets;
    }

}
