using Helpdesk.Common;
using Microsoft.AspNetCore.Http.HttpResults;

namespace Helpdesk.Features.Tickets;

public static class TicketsFeature
{
    public static IServiceCollection AddTickets(this IServiceCollection services)
    {
        services.AddSingleton<ITicketStore, InMemoryTicketStore>();
        services.AddSingleton<TicketMetrics>();
        return services;
    }

    public static RouteGroupBuilder MapTickets(this IEndpointRouteBuilder app)
    {
        var tickets = app.MapGroup("/tickets");

        tickets.MapGet("/", (ITicketStore store) => store.GetAll());

        tickets.MapGet("/{id:int}",
            Results<Ok<Ticket>, NotFound> (int id, ITicketStore store) =>
            {
                if (store.GetById(id) is { } ticket)
                {
                    return TypedResults.Ok(ticket);
                }
                return TypedResults.NotFound();
            });

        tickets.MapPost("/",
            (CreateTicketRequest request, ITicketStore store, TicketMetrics metrics, ICurrentUser user) =>
            {
                var ticket = store.Add(request.Title);
                metrics.RecordCreated(user);
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

// TODO(human): co zwraca handler operacji, ktora moze sie nie udac (lekcja 0005, zadanie 7)
