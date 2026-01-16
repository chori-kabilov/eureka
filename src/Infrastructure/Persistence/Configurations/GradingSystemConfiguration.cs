using Domain.Grading;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.Configurations;

public class GradingSystemConfiguration : IEntityTypeConfiguration<GradingSystem>
{
    public void Configure(EntityTypeBuilder<GradingSystem> builder)
    {
        builder.ToTable("grading_systems");

        builder.HasKey(e => e.Id);

        builder.Property(e => e.Name)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(e => e.MinScore)
            .HasPrecision(10, 2);

        builder.Property(e => e.MaxScore)
            .HasPrecision(10, 2);

        builder.Property(e => e.PassingScore)
            .HasPrecision(10, 2);
    }
}

public class GradingLevelConfiguration : IEntityTypeConfiguration<GradingLevel>
{
    public void Configure(EntityTypeBuilder<GradingLevel> builder)
    {
        builder.ToTable("grading_levels");

        builder.HasKey(e => e.Id);

        builder.Property(e => e.Letter)
            .IsRequired()
            .HasMaxLength(10);

        builder.Property(e => e.Description)
            .HasMaxLength(100);

        builder.Property(e => e.MinScore)
            .HasPrecision(10, 2);

        builder.Property(e => e.MaxScore)
            .HasPrecision(10, 2);

        builder.HasOne(e => e.GradingSystem)
            .WithMany(g => g.Levels)
            .HasForeignKey(e => e.GradingSystemId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
