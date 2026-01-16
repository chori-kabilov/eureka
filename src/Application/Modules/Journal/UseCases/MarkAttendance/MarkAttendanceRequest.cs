using Domain.Journal;

namespace Application.Modules.Journal.UseCases.MarkAttendance;

// Запрос отметки посещаемости
public class MarkAttendanceRequest
{
    public Guid LessonId { get; set; }
    public Guid? StudentId { get; set; }
    public Guid? ChildId { get; set; }
    public AttendanceStatus Status { get; set; }
    public TimeSpan? ArrivalTime { get; set; }
    public string? ExcuseReason { get; set; }
}
