using Domain.Schedule;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.Configurations;

public class ScheduleTemplateConfiguration : IEntityTypeConfiguration<ScheduleTemplate>
{
    public void Configure(EntityTypeBuilder<ScheduleTemplate> builder)
    {
        builder.ToTable("schedule_templates");

        builder.HasKey(e => e.Id);

        builder.Property(e => e.Id).HasColumnName("id");
        builder.Property(e => e.GroupId).HasColumnName("group_id");
        builder.Property(e => e.DayOfWeek).HasColumnName("day_of_week");
        builder.Property(e => e.StartTime).HasColumnName("start_time");
        builder.Property(e => e.EndTime).HasColumnName("end_time");
        builder.Property(e => e.RoomId).HasColumnName("room_id");
        builder.Property(e => e.DefaultLessonType).HasColumnName("default_lesson_type");
        builder.Property(e => e.IsActive).HasColumnName("is_active");
        builder.Property(e => e.ValidFrom).HasColumnName("valid_from");
        builder.Property(e => e.ValidTo).HasColumnName("valid_to");

        // Аудит
        builder.Property(e => e.CreatedAt).HasColumnName("created_at");
        builder.Property(e => e.CreatedBy).HasColumnName("created_by");
        builder.Property(e => e.UpdatedAt).HasColumnName("updated_at");
        builder.Property(e => e.UpdatedBy).HasColumnName("updated_by");
        builder.Property(e => e.IsDeleted).HasColumnName("is_deleted");
        builder.Property(e => e.DeletedAt).HasColumnName("deleted_at");

        builder.HasOne(e => e.Group)
            .WithMany(g => g.ScheduleTemplates)
            .HasForeignKey(e => e.GroupId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(e => e.Room)
            .WithMany()
            .HasForeignKey(e => e.RoomId)
            .OnDelete(DeleteBehavior.SetNull);

        builder.HasIndex(e => new { e.GroupId, e.IsActive, e.DayOfWeek });
    }
}
