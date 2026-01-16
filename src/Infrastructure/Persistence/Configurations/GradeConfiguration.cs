using Domain.Journal;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.Configurations;

public class GradeConfiguration : IEntityTypeConfiguration<Grade>
{
    public void Configure(EntityTypeBuilder<Grade> builder)
    {
        builder.ToTable("grades");

        builder.HasKey(e => e.Id);

        builder.Property(e => e.Score)
            .HasPrecision(10, 2);

        builder.Property(e => e.Weight)
            .HasPrecision(5, 2);

        builder.Property(e => e.Letter)
            .HasMaxLength(10);

        builder.Property(e => e.Comment)
            .HasMaxLength(500);

        builder.HasOne(e => e.Lesson)
            .WithMany(l => l.Grades)
            .HasForeignKey(e => e.LessonId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(e => e.Student)
            .WithMany()
            .HasForeignKey(e => e.StudentId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(e => e.Child)
            .WithMany()
            .HasForeignKey(e => e.ChildId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(e => e.GradingSystem)
            .WithMany()
            .HasForeignKey(e => e.GradingSystemId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(e => e.GradedBy)
            .WithMany()
            .HasForeignKey(e => e.GradedById)
            .OnDelete(DeleteBehavior.Restrict);

        // Индекс для поиска оценок ученика
        builder.HasIndex(e => new { e.LessonId, e.StudentId });
        builder.HasIndex(e => new { e.LessonId, e.ChildId });
    }
}
