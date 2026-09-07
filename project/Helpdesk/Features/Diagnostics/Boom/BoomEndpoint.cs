namespace Helpdesk.Features.Diagnostics.Boom;

public static class BoomEndpoint
{
    public static void Map(RouteGroupBuilder diagnostics) =>
        diagnostics.MapGet("/boom", HandleAsync).WithName("Boom");

    private static Task<string> HandleAsync() =>
        throw new InvalidOperationException("Celowy błąd, którego nikt nie obsługuje.");
}
