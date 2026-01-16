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

        builder.Property(e => e.Name)
            .IsRequired()
            .HasMaxLength(200);

        builder.Property(e => e.Code)
            .HasMaxLength(50);

        builder.Property(e => e.Notes)
            .HasMaxLength(1000);

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
            .HasFilter("\"Code\" IS NOT NULL");
    }
}
