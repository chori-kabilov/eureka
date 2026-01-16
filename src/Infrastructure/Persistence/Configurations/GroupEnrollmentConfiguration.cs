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

        builder.Property(e => e.Notes)
            .HasMaxLength(500);

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

        // Индекс для быстрого поиска активных зачислений
        builder.HasIndex(e => new { e.GroupId, e.Status });
    }
}
