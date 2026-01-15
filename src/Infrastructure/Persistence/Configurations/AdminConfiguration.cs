using Domain.Admins;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.Configurations;

// Конфигурация Admin
public class AdminConfiguration : IEntityTypeConfiguration<Admin>
{
    public void Configure(EntityTypeBuilder<Admin> builder)
    {
        builder.ToTable("admins");

        builder.HasKey(e => e.Id);

        builder.Property(e => e.Department)
            .HasMaxLength(200);

        builder.Property(e => e.Notes)
            .HasMaxLength(1000);

        builder.HasOne(e => e.User)
            .WithOne(u => u.AdminProfile)
            .HasForeignKey<Admin>(e => e.UserId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
