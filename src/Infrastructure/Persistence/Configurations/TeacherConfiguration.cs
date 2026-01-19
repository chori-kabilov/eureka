using System.Text.Json;
using Domain.Teachers;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace Infrastructure.Persistence.Configurations;

// Конфигурация Teacher
public class TeacherConfiguration : IEntityTypeConfiguration<Teacher>
{
    public void Configure(EntityTypeBuilder<Teacher> builder)
    {
        builder.ToTable("teachers");

        builder.HasKey(e => e.Id);

        builder.Property(e => e.Id).HasColumnName("id");
        builder.Property(e => e.UserId).HasColumnName("user_id");
        builder.Property(e => e.Status).HasColumnName("status");
        builder.Property(e => e.PaymentType).HasColumnName("payment_type");
        builder.Property(e => e.HourlyRate).HasColumnName("hourly_rate");
        builder.Property(e => e.HiredAt).HasColumnName("hired_at");

        // Subjects: List<string> -> JSON text
        var subjectsConverter = new ValueConverter<List<string>, string>(
            v => JsonSerializer.Serialize(v, (JsonSerializerOptions?)null),
            v => string.IsNullOrWhiteSpace(v) 
                ? new List<string>() 
                : JsonSerializer.Deserialize<List<string>>(v, (JsonSerializerOptions?)null) ?? new List<string>()
        );
        
        var subjectsComparer = new ValueComparer<List<string>>(
            (c1, c2) => c1!.SequenceEqual(c2!),
            c => c.Aggregate(0, (a, v) => HashCode.Combine(a, v.GetHashCode())),
            c => c.ToList()
        );

        builder.Property(e => e.Subjects)
            .HasColumnName("subjects")
            .HasConversion(subjectsConverter)
            .Metadata.SetValueComparer(subjectsComparer);

        builder.Property(e => e.Bio)
            .HasColumnName("bio");

        // Аудит
        builder.Property(e => e.CreatedAt).HasColumnName("created_at");
        builder.Property(e => e.CreatedBy).HasColumnName("created_by");
        builder.Property(e => e.UpdatedAt).HasColumnName("updated_at");
        builder.Property(e => e.UpdatedBy).HasColumnName("updated_by");
        builder.Property(e => e.IsDeleted).HasColumnName("is_deleted");
        builder.Property(e => e.DeletedAt).HasColumnName("deleted_at");

        builder.HasOne(e => e.User)
            .WithOne(u => u.TeacherProfile)
            .HasForeignKey<Teacher>(e => e.UserId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
