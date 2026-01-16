using Domain.Schedule;

namespace Application.Modules.Schedule.UseCases.CreateScheduleTemplate;

// Запрос на создание шаблона расписания
public class CreateScheduleTemplateRequest
{
    public Guid GroupId { get; set; }
    public int DayOfWeek { get; set; }
    public string StartTime { get; set; } = string.Empty;
    public string EndTime { get; set; } = string.Empty;
    public Guid? RoomId { get; set; }
    public LessonType DefaultLessonType { get; set; } = LessonType.Regular;
}
