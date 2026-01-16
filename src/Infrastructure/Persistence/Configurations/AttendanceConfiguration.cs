using Domain.Journal;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.Configurations;

public class AttendanceConfiguration : IEntityTypeConfiguration<Attendance>
{
    public void Configure(EntityTypeBuilder<Attendance> builder)
    {
        builder.ToTable("attendances");

        builder.HasKey(e => e.Id);

        builder.Property(e => e.ExcuseReason)
            .HasMaxLength(500);

        builder.HasOne(e => e.Lesson)
            .WithMany(l => l.Attendances)
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

        builder.HasOne(e => e.MarkedBy)
            .WithMany()
            .HasForeignKey(e => e.MarkedById)
            .OnDelete(DeleteBehavior.Restrict);

        // Уникальный индекс: один ученик — одна запись на занятие
        builder.HasIndex(e => new { e.LessonId, e.StudentId })
            .IsUnique()
            .HasFilter("\"StudentId\" IS NOT NULL");

        builder.HasIndex(e => new { e.LessonId, e.ChildId })
            .IsUnique()
            .HasFilter("\"ChildId\" IS NOT NULL");
    }
}
