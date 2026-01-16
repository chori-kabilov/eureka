using Domain.Enums;

namespace Application.Modules.Schedule.Dtos;

// DTO занятия
public class LessonDto
{
    public Guid Id { get; set; }
    public Guid GroupId { get; set; }
    public string GroupName { get; set; } = string.Empty;
    
    public DateTime Date { get; set; }
    public TimeSpan StartTime { get; set; }
    public TimeSpan EndTime { get; set; }
    
    public Guid TeacherId { get; set; }
    public string TeacherName { get; set; } = string.Empty;
    
    public Guid? RoomId { get; set; }
    public string? RoomName { get; set; }
    
    public LessonType Type { get; set; }
    public LessonStatus Status { get; set; }
    
    public string? Topic { get; set; }
    
    public int AttendanceCount { get; set; }
    public int TotalStudents { get; set; }
}

// Детальный DTO занятия
public class LessonDetailDto : LessonDto
{
    public string? Description { get; set; }
    public string? Homework { get; set; }
    
    public Guid? ReplacesLessonId { get; set; }
    public string? ReplacementReason { get; set; }
    
    public string? CancellationReason { get; set; }
    
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
}
