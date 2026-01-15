using Domain.Teachers;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.Configurations;

// Конфигурация Teacher
public class TeacherConfiguration : IEntityTypeConfiguration<Teacher>
{
    public void Configure(EntityTypeBuilder<Teacher> builder)
    {
        builder.ToTable("teachers");

        builder.HasKey(e => e.Id);

        builder.Property(e => e.Specialization)
            .HasMaxLength(200);

        builder.Property(e => e.Bio)
            .HasMaxLength(1000);

        builder.Property(e => e.HourlyRate)
            .HasPrecision(18, 2);

        builder.HasOne(e => e.User)
            .WithOne(u => u.TeacherProfile)
            .HasForeignKey<Teacher>(e => e.UserId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
