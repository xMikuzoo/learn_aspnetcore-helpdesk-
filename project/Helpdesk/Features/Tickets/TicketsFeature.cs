using Helpdesk.Common;
using Helpdesk.Features.Tickets.Commands.CreateTicket;
using Helpdesk.Features.Tickets.Commands.DeleteTicket;
using Helpdesk.Features.Tickets.Commands.UpdateTicket;
using Helpdesk.Features.Tickets.Queries.GetTicket;
using Helpdesk.Features.Tickets.Queries.GetTicketMetrics;
using Helpdesk.Features.Tickets.Queries.ListTickets;

namespace Helpdesk.Features.Tickets;

public static class TicketsFeature
{
    public static IServiceCollection AddTickets(this IServiceCollection services)
    {
        services.AddSingleton<ITicketStore, InMemoryTicketStore>();
        services.AddSingleton<TicketMetrics>();

        services.AddScoped<ICommandHandler<CreateTicketCommand, TicketDto>, CreateTicketHandler>();
        services.AddScoped<ICommandHandler<UpdateTicketCommand, bool>, UpdateTicketHandler>();
        services.AddScoped<ICommandHandler<DeleteTicketCommand, bool>, DeleteTicketHandler>();

        services.AddScoped<IQueryHandler<GetTicketQuery, TicketDto?>, GetTicketHandler>();
        services.AddScoped<IQueryHandler<ListTicketsQuery, IReadOnlyList<TicketDto>>, ListTicketsHandler>();
        services.AddScoped<IQueryHandler<GetTicketMetricsQuery, TicketMetricsDto>, GetTicketMetricsHandler>();

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
        DeleteTicketEndpoint.Map(tickets);

        return tickets;
    }
}
