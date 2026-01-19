using Domain.Groups;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.Configurations;

public class GroupConfiguration : IEntityTypeConfiguration<Group>
{
    public void Configure(EntityTypeBuilder<Group> builder)
    {
        builder.ToTable("groups");

        builder.HasKey(e => e.Id);

        builder.Property(e => e.Id).HasColumnName("id");
        
        builder.Property(e => e.Name)
            .HasColumnName("name")
            .IsRequired()
            .HasMaxLength(200);

        builder.Property(e => e.Code)
            .HasColumnName("code")
            .HasMaxLength(50);

        builder.Property(e => e.CourseId)
            .HasColumnName("course_id");

        builder.Property(e => e.ResponsibleTeacherId)
            .HasColumnName("responsible_teacher_id");

        builder.Property(e => e.DefaultTeacherId)
            .HasColumnName("default_teacher_id");

        builder.Property(e => e.DefaultRoomId)
            .HasColumnName("default_room_id");

        builder.Property(e => e.GradingSystemId)
            .HasColumnName("grading_system_id");

        builder.Property(e => e.StartDate)
            .HasColumnName("start_date");

        builder.Property(e => e.EndDate)
            .HasColumnName("end_date");

        builder.Property(e => e.MaxStudents)
            .HasColumnName("max_students");

        builder.Property(e => e.Status)
            .HasColumnName("status");

        builder.Property(e => e.Notes)
            .HasColumnName("notes")
            .HasMaxLength(1000);

        // Аудит
        builder.Property(e => e.CreatedAt).HasColumnName("created_at");
        builder.Property(e => e.CreatedBy).HasColumnName("created_by");
        builder.Property(e => e.UpdatedAt).HasColumnName("updated_at");
        builder.Property(e => e.UpdatedBy).HasColumnName("updated_by");
        builder.Property(e => e.IsDeleted).HasColumnName("is_deleted");
        builder.Property(e => e.DeletedAt).HasColumnName("deleted_at");

        builder.HasOne(e => e.Course)
            .WithMany()
            .HasForeignKey(e => e.CourseId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(e => e.ResponsibleTeacher)
            .WithMany()
            .HasForeignKey(e => e.ResponsibleTeacherId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(e => e.DefaultTeacher)
            .WithMany()
            .HasForeignKey(e => e.DefaultTeacherId)
            .OnDelete(DeleteBehavior.SetNull);

        builder.HasOne(e => e.DefaultRoom)
            .WithMany()
            .HasForeignKey(e => e.DefaultRoomId)
            .OnDelete(DeleteBehavior.SetNull);

        builder.HasOne(e => e.GradingSystem)
            .WithMany()
            .HasForeignKey(e => e.GradingSystemId)
            .OnDelete(DeleteBehavior.SetNull);

        builder.HasIndex(e => e.Code)
            .IsUnique()
            .HasFilter("\"code\" IS NOT NULL");
    }
}
