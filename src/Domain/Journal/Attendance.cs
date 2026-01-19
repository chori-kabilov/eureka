using Domain.Common;
using Domain.Schedule;
using Domain.Students;
using Domain.Users;

namespace Domain.Journal;

// Посещаемость
public class Attendance : BaseEntity
{
    public Guid LessonId { get; set; }
    public Lesson Lesson { get; set; } = null!;
    
    // Студент
    public Guid? StudentId { get; set; }
    public Student? Student { get; set; }
    
    // Ребёнок
    public Guid? ChildId { get; set; }
    public Child? Child { get; set; }
    
    public AttendanceStatus Status { get; set; } = AttendanceStatus.Present;
    
    // Если опоздал
    public TimeSpan? ArrivalTime { get; set; }
    
    // Если ушёл раньше
    public TimeSpan? LeaveTime { get; set; }
    
    // Причина уважительного пропуска
    public string? ExcuseReason { get; set; }
    
    // Кто отметил
    public Guid MarkedById { get; set; }
    public User MarkedBy { get; set; } = null!;
    public DateTime MarkedAt { get; set; }
}
