using Helpdesk.Features.Diagnostics.Boom;
using Helpdesk.Features.Diagnostics.Lifetimes;

namespace Helpdesk.Features.Diagnostics;

public static class DiagnosticsFeature
{
    public static IServiceCollection AddDiagnostics(this IServiceCollection services)
    {
        services.AddTransient<ITransientStamp, Stamp>();
        services.AddScoped<IScopedStamp, Stamp>();
        services.AddSingleton<ISingletonStamp, Stamp>();
        return services;
    }

    public static RouteGroupBuilder MapDiagnostics(this IEndpointRouteBuilder app)
    {
        var diagnostics = app.MapGroup("/diagnostics").WithTags("Diagnostics");

        LifetimesEndpoint.Map(diagnostics);
        BoomEndpoint.Map(diagnostics);

        return diagnostics;
    }
}
