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

        builder.Property(e => e.Balance)
            .HasPrecision(18, 2);

        builder.HasOne(e => e.User)
            .WithOne(u => u.StudentProfile)
            .HasForeignKey<Student>(e => e.UserId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(e => e.Child)
            .WithOne(c => c.LinkedStudent)
            .HasForeignKey<Student>(e => e.ChildId)
            .OnDelete(DeleteBehavior.SetNull);
    }
}
