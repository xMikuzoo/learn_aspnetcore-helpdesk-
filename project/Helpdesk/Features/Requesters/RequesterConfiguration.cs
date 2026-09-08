using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Helpdesk.Features.Requesters;

public class RequesterConfiguration : IEntityTypeConfiguration<Requester>
{
    public void Configure(EntityTypeBuilder<Requester> builder)
    {
        builder.HasKey(r => r.Id);
        builder.Property(r => r.Login).IsRequired().HasMaxLength(50);
        builder.Property(r => r.DisplayName).IsRequired().HasMaxLength(100);

        builder.HasIndex(r => r.Login).IsUnique();

        builder.HasData(
            new { Id = 1, Login = "wojtek", DisplayName = "Wojciech Król" },
            new { Id = 2, Login = "ania", DisplayName = "Anna Nowak" });
    }
}
