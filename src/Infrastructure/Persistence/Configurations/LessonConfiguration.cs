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

        builder.Property(e => e.Topic)
            .HasMaxLength(300);

        builder.Property(e => e.Description)
            .HasMaxLength(2000);

        builder.Property(e => e.Homework)
            .HasMaxLength(1000);

        builder.Property(e => e.ReplacementReason)
            .HasMaxLength(500);

        builder.Property(e => e.CancellationReason)
            .HasMaxLength(500);

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

        // Индексы для быстрого поиска
        builder.HasIndex(e => new { e.GroupId, e.Date });
        builder.HasIndex(e => new { e.TeacherId, e.Date });
        builder.HasIndex(e => e.Date);
    }
}
