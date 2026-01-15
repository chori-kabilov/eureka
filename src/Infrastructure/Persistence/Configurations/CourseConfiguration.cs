using Domain.Courses;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.Configurations;

// Конфигурация таблицы Courses
public class CourseConfiguration : IEntityTypeConfiguration<Course>
{
    public void Configure(EntityTypeBuilder<Course> builder)
    {
        builder.ToTable("courses");

        builder.HasKey(c => c.Id);

        builder.Property(c => c.Id)
            .HasColumnName("id");

        builder.Property(c => c.Name)
            .HasColumnName("name")
            .HasMaxLength(200)
            .IsRequired();

        builder.Property(c => c.Description)
            .HasColumnName("description");

        builder.Property(c => c.StudentPaymentType)
            .HasColumnName("student_payment_type")
            .IsRequired();

        builder.Property(c => c.AbsencePolicy)
            .HasColumnName("absence_policy")
            .IsRequired();

        builder.Property(c => c.TeacherPaymentType)
            .HasColumnName("teacher_payment_type")
            .IsRequired();

        builder.Property(c => c.Status)
            .HasColumnName("status")
            .IsRequired();

        // Audit fields
        builder.Property(c => c.CreatedAt)
            .HasColumnName("created_at")
            .IsRequired();

        builder.Property(c => c.CreatedBy)
            .HasColumnName("created_by");

        builder.Property(c => c.UpdatedAt)
            .HasColumnName("updated_at");

        builder.Property(c => c.UpdatedBy)
            .HasColumnName("updated_by");

        // Soft delete fields
        builder.Property(c => c.IsDeleted)
            .HasColumnName("is_deleted")
            .HasDefaultValue(false)
            .IsRequired();

        builder.Property(c => c.DeletedAt)
            .HasColumnName("deleted_at");

        // Indexes
        builder.HasIndex(c => c.Status)
            .HasDatabaseName("idx_courses_status");

        builder.HasIndex(c => c.IsDeleted)
            .HasDatabaseName("idx_courses_is_deleted");
    }
}
