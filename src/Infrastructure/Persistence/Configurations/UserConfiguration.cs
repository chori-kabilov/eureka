using Domain.Users;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.Configurations;

// Конфигурация User
public class UserConfiguration : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.ToTable("users");

        builder.HasKey(u => u.Id);

        builder.Property(u => u.Id).HasColumnName("id");

        builder.Property(u => u.Phone)
            .HasColumnName("phone")
            .HasMaxLength(20)
            .IsRequired();

        builder.Property(u => u.FullName)
            .HasColumnName("full_name")
            .HasMaxLength(200)
            .IsRequired();

        builder.Property(u => u.PasswordHash)
            .HasColumnName("password_hash")
            .IsRequired();

        builder.Property(u => u.BirthDate)
            .HasColumnName("birth_date");

        builder.Property(u => u.Gender)
            .HasColumnName("gender");

        builder.Property(u => u.Nationality)
            .HasColumnName("nationality")
            .HasMaxLength(100);

        builder.Property(u => u.Address)
            .HasColumnName("address")
            .HasMaxLength(500);

        builder.Property(u => u.ProfilePhotoUrl)
            .HasColumnName("profile_photo_url")
            .HasMaxLength(500);

        // Аудит
        builder.Property(u => u.CreatedAt).HasColumnName("created_at");
        builder.Property(u => u.CreatedBy).HasColumnName("created_by");
        builder.Property(u => u.UpdatedAt).HasColumnName("updated_at");
        builder.Property(u => u.UpdatedBy).HasColumnName("updated_by");
        builder.Property(u => u.IsDeleted).HasColumnName("is_deleted").HasDefaultValue(false);
        builder.Property(u => u.DeletedAt).HasColumnName("deleted_at");

        builder.HasIndex(u => u.Phone).IsUnique().HasDatabaseName("idx_users_phone");
        builder.HasIndex(u => u.IsDeleted).HasDatabaseName("idx_users_is_deleted");

        builder.HasQueryFilter(u => !u.IsDeleted);
    }
}
