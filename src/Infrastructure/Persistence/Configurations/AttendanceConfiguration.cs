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

        builder.Property(e => e.Id).HasColumnName("id");
        builder.Property(e => e.LessonId).HasColumnName("lesson_id");
        builder.Property(e => e.StudentId).HasColumnName("student_id");
        builder.Property(e => e.ChildId).HasColumnName("child_id");
        builder.Property(e => e.Status).HasColumnName("status");
        builder.Property(e => e.ArrivalTime).HasColumnName("arrival_time");
        builder.Property(e => e.LeaveTime).HasColumnName("leave_time");
        builder.Property(e => e.MarkedById).HasColumnName("marked_by_id");
        builder.Property(e => e.MarkedAt).HasColumnName("marked_at");
        
        builder.Property(e => e.ExcuseReason)
            .HasColumnName("excuse_reason")
            .HasMaxLength(500);

        // Аудит
        builder.Property(e => e.CreatedAt).HasColumnName("created_at");
        builder.Property(e => e.CreatedBy).HasColumnName("created_by");
        builder.Property(e => e.UpdatedAt).HasColumnName("updated_at");
        builder.Property(e => e.UpdatedBy).HasColumnName("updated_by");
        builder.Property(e => e.IsDeleted).HasColumnName("is_deleted");
        builder.Property(e => e.DeletedAt).HasColumnName("deleted_at");

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

        builder.HasIndex(e => new { e.LessonId, e.StudentId })
            .IsUnique()
            .HasFilter("\"student_id\" IS NOT NULL");

        builder.HasIndex(e => new { e.LessonId, e.ChildId })
            .IsUnique()
            .HasFilter("\"child_id\" IS NOT NULL");
    }
}
