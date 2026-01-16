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

        builder.HasOne(e => e.Group)
            .WithMany(g => g.ScheduleTemplates)
            .HasForeignKey(e => e.GroupId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(e => e.Room)
            .WithMany()
            .HasForeignKey(e => e.RoomId)
            .OnDelete(DeleteBehavior.SetNull);

        // Индекс для активных шаблонов группы
        builder.HasIndex(e => new { e.GroupId, e.IsActive, e.DayOfWeek });
    }
}
