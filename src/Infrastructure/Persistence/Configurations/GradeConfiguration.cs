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

        builder.Property(e => e.Id).HasColumnName("id");
        builder.Property(e => e.LessonId).HasColumnName("lesson_id");
        builder.Property(e => e.StudentId).HasColumnName("student_id");
        builder.Property(e => e.ChildId).HasColumnName("child_id");
        builder.Property(e => e.GradingSystemId).HasColumnName("grading_system_id");
        builder.Property(e => e.GradedById).HasColumnName("graded_by_id");
        builder.Property(e => e.GradedAt).HasColumnName("graded_at");

        builder.Property(e => e.Score)
            .HasColumnName("score")
            .HasPrecision(10, 2);

        builder.Property(e => e.Weight)
            .HasColumnName("weight")
            .HasPrecision(5, 2);

        builder.Property(e => e.Letter)
            .HasColumnName("letter")
            .HasMaxLength(10);

        builder.Property(e => e.Comment)
            .HasColumnName("comment")
            .HasMaxLength(500);

        // Аудит
        builder.Property(e => e.CreatedAt).HasColumnName("created_at");
        builder.Property(e => e.CreatedBy).HasColumnName("created_by");
        builder.Property(e => e.UpdatedAt).HasColumnName("updated_at");
        builder.Property(e => e.UpdatedBy).HasColumnName("updated_by");
        builder.Property(e => e.IsDeleted).HasColumnName("is_deleted");
        builder.Property(e => e.DeletedAt).HasColumnName("deleted_at");

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

        builder.HasIndex(e => new { e.LessonId, e.StudentId });
        builder.HasIndex(e => new { e.LessonId, e.ChildId });
    }
}
