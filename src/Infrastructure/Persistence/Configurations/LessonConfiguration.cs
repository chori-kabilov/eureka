using Domain.Schedule;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.Configurations;

public class LessonConfiguration : IEntityTypeConfiguration<Lesson>
{
    public void Configure(EntityTypeBuilder<Lesson> builder)
    {
        builder.ToTable("lessons");

        builder.HasKey(e => e.Id);

        builder.Property(e => e.Id).HasColumnName("id");
        builder.Property(e => e.GroupId).HasColumnName("group_id");
        builder.Property(e => e.ScheduleTemplateId).HasColumnName("schedule_template_id");
        builder.Property(e => e.Date).HasColumnName("date");
        builder.Property(e => e.StartTime).HasColumnName("start_time");
        builder.Property(e => e.EndTime).HasColumnName("end_time");
        builder.Property(e => e.RoomId).HasColumnName("room_id");
        builder.Property(e => e.TeacherId).HasColumnName("teacher_id");
        builder.Property(e => e.Type).HasColumnName("type");
        builder.Property(e => e.Status).HasColumnName("status");
        builder.Property(e => e.ReplacesLessonId).HasColumnName("replaces_lesson_id");
        builder.Property(e => e.OriginalCourseId).HasColumnName("original_course_id");
        builder.Property(e => e.RescheduledToLessonId).HasColumnName("rescheduled_to_lesson_id");
        
        builder.Property(e => e.Topic)
            .HasColumnName("topic")
            .HasMaxLength(300);

        builder.Property(e => e.Description)
            .HasColumnName("description")
            .HasMaxLength(2000);

        builder.Property(e => e.Homework)
            .HasColumnName("homework")
            .HasMaxLength(1000);

        builder.Property(e => e.ReplacementReason)
            .HasColumnName("replacement_reason")
            .HasMaxLength(500);

        builder.Property(e => e.CancellationReason)
            .HasColumnName("cancellation_reason")
            .HasMaxLength(500);

        // Аудит
        builder.Property(e => e.CreatedAt).HasColumnName("created_at");
        builder.Property(e => e.CreatedBy).HasColumnName("created_by");
        builder.Property(e => e.UpdatedAt).HasColumnName("updated_at");
        builder.Property(e => e.UpdatedBy).HasColumnName("updated_by");
        builder.Property(e => e.IsDeleted).HasColumnName("is_deleted");
        builder.Property(e => e.DeletedAt).HasColumnName("deleted_at");

        builder.HasOne(e => e.Group)
            .WithMany(g => g.Lessons)
            .HasForeignKey(e => e.GroupId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(e => e.ScheduleTemplate)
            .WithMany()
            .HasForeignKey(e => e.ScheduleTemplateId)
            .OnDelete(DeleteBehavior.SetNull);

        builder.HasOne(e => e.Room)
            .WithMany()
            .HasForeignKey(e => e.RoomId)
            .OnDelete(DeleteBehavior.SetNull);

        builder.HasOne(e => e.Teacher)
            .WithMany()
            .HasForeignKey(e => e.TeacherId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(e => e.ReplacesLesson)
            .WithMany()
            .HasForeignKey(e => e.ReplacesLessonId)
            .OnDelete(DeleteBehavior.SetNull);

        builder.HasOne(e => e.OriginalCourse)
            .WithMany()
            .HasForeignKey(e => e.OriginalCourseId)
            .OnDelete(DeleteBehavior.SetNull);

        builder.HasOne(e => e.RescheduledToLesson)
            .WithMany()
            .HasForeignKey(e => e.RescheduledToLessonId)
            .OnDelete(DeleteBehavior.SetNull);

        builder.HasIndex(e => new { e.GroupId, e.Date });
        builder.HasIndex(e => new { e.TeacherId, e.Date });
        builder.HasIndex(e => e.Date);
    }
}
