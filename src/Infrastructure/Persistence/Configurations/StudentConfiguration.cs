using Domain.Students;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.Configurations;

// Конфигурация Student
public class StudentConfiguration : IEntityTypeConfiguration<Student>
{
    public void Configure(EntityTypeBuilder<Student> builder)
    {
        builder.ToTable("students");

        builder.HasKey(e => e.Id);

        builder.Property(e => e.Id).HasColumnName("id");
        builder.Property(e => e.UserId).HasColumnName("user_id");
        builder.Property(e => e.Status).HasColumnName("status");
        builder.Property(e => e.Notes).HasColumnName("notes");
        builder.Property(e => e.ChildId).HasColumnName("child_id");

        builder.Property(e => e.EnrollmentDate)
            .HasColumnName("enrollment_date")
            .IsRequired();

        builder.Property(e => e.EducationLevel)
            .HasColumnName("education_level");

        // Аудит
        builder.Property(e => e.CreatedAt).HasColumnName("created_at");
        builder.Property(e => e.CreatedBy).HasColumnName("created_by");
        builder.Property(e => e.UpdatedAt).HasColumnName("updated_at");
        builder.Property(e => e.UpdatedBy).HasColumnName("updated_by");
        builder.Property(e => e.IsDeleted).HasColumnName("is_deleted");
        builder.Property(e => e.DeletedAt).HasColumnName("deleted_at");

        // Связь Student -> User (one-to-one)
        builder.HasOne(e => e.User)
            .WithOne(u => u.StudentProfile)
            .HasForeignKey<Student>(e => e.UserId)
            .OnDelete(DeleteBehavior.Cascade);

        // Связь Student -> Child (one-to-one, Student зависит от Child)
        // Student.ChildId — внешний ключ, указывающий на Child
        builder.HasOne(e => e.Child)
            .WithOne(c => c.LinkedStudent)
            .HasForeignKey<Student>(e => e.ChildId)
            .OnDelete(DeleteBehavior.SetNull);
    }
}
