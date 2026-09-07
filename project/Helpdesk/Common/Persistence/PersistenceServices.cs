using Microsoft.EntityFrameworkCore;

namespace Helpdesk.Common.Persistence;

public static class PersistenceServices
{
    public static IServiceCollection AddPersistence(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<HelpdeskDbContext>(options =>
            options.UseSqlite(configuration.GetConnectionString("Helpdesk")));

        return services;
    }
}
