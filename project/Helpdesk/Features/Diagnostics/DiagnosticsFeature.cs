
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
        var diagnostics = app.MapGroup("/diagnostics");

        diagnostics.MapGet("/lifetimes", (
            ITransientStamp t1, ITransientStamp t2,
            IScopedStamp s1, IScopedStamp s2,
            ISingletonStamp g1) => new
            {
                transient1 = t1.Id,
                transient2 = t2.Id,
                scoped1 = s1.Id,
                scoped2 = s2.Id,
                singleton = g1.Id
            });

        return diagnostics;
    }

}
