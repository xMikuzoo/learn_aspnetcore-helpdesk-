namespace Helpdesk.Common;

public static class CommonServices
{
    public static IServiceCollection AddCurrentUser(this IServiceCollection services)
    {
        services.AddHttpContextAccessor();
        services.AddScoped<ICurrentUser, HeaderCurrentUser>();

        return services;
    }
}
