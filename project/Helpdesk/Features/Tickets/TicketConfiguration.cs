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
        builder.Property(t => t.Status).IsRequired().HasMaxLength(20).HasConversion<string>();

        builder
            .HasOne(t => t.Requester)
            .WithMany()
            .HasForeignKey(t => t.RequesterId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasIndex(t => new
        {
            t.Title,
            t.RequesterId
        }).IsUnique().HasFilter("\"Status\" <> 'Resolved'");

        builder.HasData(
            new { Id = 1, Title = "Pierwsze zgłoszenie", Priority = "normal", Status = TicketStatus.Open, RequesterId = 1 },
            new { Id = 2, Title = "Drugie zgłoszenie!", Priority = "high", Status = TicketStatus.Open, RequesterId = 2 });
    }
}
