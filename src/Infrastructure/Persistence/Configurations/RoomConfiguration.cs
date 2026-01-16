using Domain.Rooms;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.Configurations;

public class RoomConfiguration : IEntityTypeConfiguration<Room>
{
    public void Configure(EntityTypeBuilder<Room> builder)
    {
        builder.ToTable("rooms");

        builder.HasKey(e => e.Id);

        builder.Property(e => e.Name)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(e => e.Code)
            .HasMaxLength(20);

        builder.Property(e => e.Description)
            .HasMaxLength(500);

        builder.HasIndex(e => e.Code)
            .IsUnique()
            .HasFilter("\"Code\" IS NOT NULL");
    }
}
