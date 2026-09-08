using Helpdesk.Features.Tickets.Commands.CreateTicket;
using Helpdesk.Features.Tickets.Commands.DeleteTicket;
using Helpdesk.Features.Tickets.Commands.ResolveTicket;
using Helpdesk.Features.Tickets.Commands.UpdateTicket;
using Helpdesk.Features.Tickets.Queries.GetTicket;
using Helpdesk.Features.Tickets.Queries.GetTicketMetrics;
using Helpdesk.Features.Tickets.Queries.ListTickets;

namespace Helpdesk.Features.Tickets;

public static class TicketsFeature
{
    public static IServiceCollection AddTickets(this IServiceCollection services)
    {
        services.AddScoped<ITicketStore, EfTicketStore>();
        services.AddSingleton<TicketMetrics>();

        return services;
    }

    public static RouteGroupBuilder MapTickets(this IEndpointRouteBuilder app)
    {
        var tickets = app.MapGroup("/tickets").WithTags("Tickets");

        ListTicketsEndpoint.Map(tickets);
        GetTicketEndpoint.Map(tickets);
        GetTicketMetricsEndpoint.Map(tickets);
        CreateTicketEndpoint.Map(tickets);
        UpdateTicketEndpoint.Map(tickets);
        ResolveTicketEndpoint.Map(tickets);
        DeleteTicketEndpoint.Map(tickets);

        return tickets;
    }
}
