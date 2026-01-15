using Domain.Parents;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.Configurations;

// Конфигурация Parent
public class ParentConfiguration : IEntityTypeConfiguration<Parent>
{
    public void Configure(EntityTypeBuilder<Parent> builder)
    {
        builder.ToTable("parents");

        builder.HasKey(e => e.Id);

        builder.HasOne(e => e.User)
            .WithOne(u => u.ParentProfile)
            .HasForeignKey<Parent>(e => e.UserId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
