using Domain.Grading;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.Configurations;

public class GradingLevelConfiguration : IEntityTypeConfiguration<GradingLevel>
{
    public void Configure(EntityTypeBuilder<GradingLevel> builder)
    {
        builder.ToTable("grading_levels");

        builder.HasKey(e => e.Id);

        builder.Property(e => e.Id).HasColumnName("id");
        builder.Property(e => e.GradingSystemId).HasColumnName("grading_system_id");
        builder.Property(e => e.Order).HasColumnName("order");

        builder.Property(e => e.Letter)
            .HasColumnName("letter")
            .IsRequired()
            .HasMaxLength(10);

        builder.Property(e => e.Description)
            .HasColumnName("description")
            .HasMaxLength(100);

        builder.Property(e => e.MinScore)
            .HasColumnName("min_score")
            .HasPrecision(10, 2);

        builder.Property(e => e.MaxScore)
            .HasColumnName("max_score")
            .HasPrecision(10, 2);

        // Аудит
        builder.Property(e => e.CreatedAt).HasColumnName("created_at");
        builder.Property(e => e.CreatedBy).HasColumnName("created_by");
        builder.Property(e => e.UpdatedAt).HasColumnName("updated_at");
        builder.Property(e => e.UpdatedBy).HasColumnName("updated_by");
        builder.Property(e => e.IsDeleted).HasColumnName("is_deleted");
        builder.Property(e => e.DeletedAt).HasColumnName("deleted_at");

        builder.HasOne(e => e.GradingSystem)
            .WithMany(g => g.Levels)
            .HasForeignKey(e => e.GradingSystemId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
