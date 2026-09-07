using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Helpdesk.Features.Tickets;

public class TicketConfiguration : IEntityTypeConfiguration<Ticket>
{
    public void Configure(EntityTypeBuilder<Ticket> builder)
    {
        builder.HasKey(t => t.Id);
        builder.Property(t => t.Title).IsRequired().HasMaxLength(100);
        builder.Property(t => t.Priority).IsRequired().HasMaxLength(20);

        builder.HasData(
            new { Id = 1, Title = "Pierwsze zgłoszenie", Priority = "normal" },
            new { Id = 2, Title = "Drugie zgłoszenie!", Priority = "high" });
    }
}
