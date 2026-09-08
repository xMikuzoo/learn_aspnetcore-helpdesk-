using Helpdesk.Features.Requesters;
using Helpdesk.Features.Tickets;
using Microsoft.EntityFrameworkCore;

namespace Helpdesk.Common.Persistence;

public class HelpdeskDbContext(DbContextOptions<HelpdeskDbContext> options) : DbContext(options)
{
    public DbSet<Ticket> Tickets => Set<Ticket>();
    public DbSet<Requester> Requesters => Set<Requester>();

    protected override void OnModelCreating(ModelBuilder modelBuilder) =>
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(HelpdeskDbContext).Assembly);
}
