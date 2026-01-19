using Domain.Schedule;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.Configurations;

public class LessonAssistantConfiguration : IEntityTypeConfiguration<LessonAssistant>
{
    public void Configure(EntityTypeBuilder<LessonAssistant> builder)
    {
        builder.ToTable("lesson_assistants");

        builder.HasKey(e => new { e.LessonId, e.TeacherId, e.StudentId, e.ChildId });

        builder.Property(e => e.LessonId).HasColumnName("lesson_id");
        builder.Property(e => e.TeacherId).HasColumnName("teacher_id");
        builder.Property(e => e.StudentId).HasColumnName("student_id");
        builder.Property(e => e.ChildId).HasColumnName("child_id");
        
        builder.Property(e => e.Role)
            .HasColumnName("role")
            .HasMaxLength(100);

        builder.HasOne(e => e.Lesson)
            .WithMany(l => l.Assistants)
            .HasForeignKey(e => e.LessonId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(e => e.Teacher)
            .WithMany()
            .HasForeignKey(e => e.TeacherId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(e => e.Student)
            .WithMany()
            .HasForeignKey(e => e.StudentId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(e => e.Child)
            .WithMany()
            .HasForeignKey(e => e.ChildId)
            .IsRequired(false)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
