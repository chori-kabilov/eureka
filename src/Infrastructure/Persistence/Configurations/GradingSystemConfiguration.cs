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

        builder.Property(e => e.Id).HasColumnName("id");
        builder.Property(e => e.Type).HasColumnName("type");
        builder.Property(e => e.IsDefault).HasColumnName("is_default");

        builder.Property(e => e.Name)
            .HasColumnName("name")
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(e => e.MinScore)
            .HasColumnName("min_score")
            .HasPrecision(10, 2);

        builder.Property(e => e.MaxScore)
            .HasColumnName("max_score")
            .HasPrecision(10, 2);

        builder.Property(e => e.PassingScore)
            .HasColumnName("passing_score")
            .HasPrecision(10, 2);

        // Аудит
        builder.Property(e => e.CreatedAt).HasColumnName("created_at");
        builder.Property(e => e.CreatedBy).HasColumnName("created_by");
        builder.Property(e => e.UpdatedAt).HasColumnName("updated_at");
        builder.Property(e => e.UpdatedBy).HasColumnName("updated_by");
        builder.Property(e => e.IsDeleted).HasColumnName("is_deleted");
        builder.Property(e => e.DeletedAt).HasColumnName("deleted_at");
    }
}
