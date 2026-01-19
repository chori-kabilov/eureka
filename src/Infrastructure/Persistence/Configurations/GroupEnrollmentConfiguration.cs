using Domain.Groups;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.Configurations;

public class GroupEnrollmentConfiguration : IEntityTypeConfiguration<GroupEnrollment>
{
    public void Configure(EntityTypeBuilder<GroupEnrollment> builder)
    {
        builder.ToTable("group_enrollments");

        builder.HasKey(e => e.Id);

        builder.Property(e => e.Id).HasColumnName("id");
        builder.Property(e => e.GroupId).HasColumnName("group_id");
        builder.Property(e => e.StudentId).HasColumnName("student_id");
        builder.Property(e => e.ChildId).HasColumnName("child_id");
        builder.Property(e => e.EnrolledAt).HasColumnName("enrolled_at");
        builder.Property(e => e.LeftAt).HasColumnName("left_at");
        builder.Property(e => e.Status).HasColumnName("status");
        builder.Property(e => e.TransferredFromGroupId).HasColumnName("transferred_from_group_id");
        builder.Property(e => e.TransferredToGroupId).HasColumnName("transferred_to_group_id");
        
        builder.Property(e => e.Notes)
            .HasColumnName("notes")
            .HasMaxLength(500);

        // Аудит
        builder.Property(e => e.CreatedAt).HasColumnName("created_at");
        builder.Property(e => e.CreatedBy).HasColumnName("created_by");
        builder.Property(e => e.UpdatedAt).HasColumnName("updated_at");
        builder.Property(e => e.UpdatedBy).HasColumnName("updated_by");
        builder.Property(e => e.IsDeleted).HasColumnName("is_deleted");
        builder.Property(e => e.DeletedAt).HasColumnName("deleted_at");

        builder.HasOne(e => e.Group)
            .WithMany(g => g.Enrollments)
            .HasForeignKey(e => e.GroupId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(e => e.Student)
            .WithMany()
            .HasForeignKey(e => e.StudentId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(e => e.Child)
            .WithMany()
            .HasForeignKey(e => e.ChildId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(e => e.TransferredFromGroup)
            .WithMany()
            .HasForeignKey(e => e.TransferredFromGroupId)
            .OnDelete(DeleteBehavior.SetNull);

        builder.HasOne(e => e.TransferredToGroup)
            .WithMany()
            .HasForeignKey(e => e.TransferredToGroupId)
            .OnDelete(DeleteBehavior.SetNull);

        builder.HasIndex(e => new { e.GroupId, e.Status });
    }
}
