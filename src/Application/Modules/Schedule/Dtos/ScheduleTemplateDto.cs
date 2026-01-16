using Domain.Schedule;

namespace Application.Modules.Schedule.Dtos;

// DTO шаблона расписания
public class ScheduleTemplateDto
{
    public Guid Id { get; set; }
    public Guid GroupId { get; set; }
    public DayOfWeek DayOfWeek { get; set; }
    public TimeSpan StartTime { get; set; }
    public TimeSpan EndTime { get; set; }
    public Guid? RoomId { get; set; }
    public string? RoomName { get; set; }
    public LessonType DefaultLessonType { get; set; }
    public bool IsActive { get; set; }
}
