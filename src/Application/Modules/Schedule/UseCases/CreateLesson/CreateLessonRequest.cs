using Domain.Schedule;

namespace Application.Modules.Schedule.UseCases.CreateLesson;

// Запрос на создание занятия
public class CreateLessonRequest
{
    public Guid GroupId { get; set; }
    public DateTime Date { get; set; }
    public TimeSpan StartTime { get; set; }
    public TimeSpan EndTime { get; set; }
    public Guid TeacherId { get; set; }
    public Guid? RoomId { get; set; }
    public LessonType Type { get; set; } = LessonType.Regular;
    public string? Topic { get; set; }
    public string? Description { get; set; }
}
