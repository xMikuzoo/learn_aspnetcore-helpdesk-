namespace Helpdesk.Features.Diagnostics.Lifetimes;

public static class LifetimesEndpoint
{
    public static void Map(RouteGroupBuilder diagnostics) =>
        diagnostics.MapGet("/lifetimes", HandleAsync).WithName("Lifetimes");

    private static IResult HandleAsync(
        ITransientStamp t1, ITransientStamp t2,
        IScopedStamp s1, IScopedStamp s2,
        ISingletonStamp g1) =>
        TypedResults.Ok(new
        {
            transient1 = t1.Id,
            transient2 = t2.Id,
            scoped1 = s1.Id,
            scoped2 = s2.Id,
            singleton = g1.Id
        });
}
