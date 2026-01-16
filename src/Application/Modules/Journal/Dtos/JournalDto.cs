using Domain.Enums;

namespace Application.Modules.Journal.Dtos;

// DTO посещаемости
public class AttendanceDto
{
    public Guid Id { get; set; }
    public Guid LessonId { get; set; }
    
    public Guid? StudentId { get; set; }
    public Guid? ChildId { get; set; }
    public string StudentName { get; set; } = string.Empty;
    
    public AttendanceStatus Status { get; set; }
    public TimeSpan? ArrivalTime { get; set; }
    public TimeSpan? LeaveTime { get; set; }
    public string? ExcuseReason { get; set; }
    
    public DateTime MarkedAt { get; set; }
    public string MarkedByName { get; set; } = string.Empty;
}

// DTO оценки
public class GradeDto
{
    public Guid Id { get; set; }
    public Guid LessonId { get; set; }
    
    public Guid? StudentId { get; set; }
    public Guid? ChildId { get; set; }
    public string StudentName { get; set; } = string.Empty;
    
    public decimal Score { get; set; }
    public string? Letter { get; set; }
    public decimal Weight { get; set; }
    public string? Comment { get; set; }
    
    public Guid GradingSystemId { get; set; }
    public string GradingSystemName { get; set; } = string.Empty;
    
    public DateTime GradedAt { get; set; }
    public string GradedByName { get; set; } = string.Empty;
}

// DTO для строки журнала (ученик + все занятия)
public class JournalRowDto
{
    public Guid? StudentId { get; set; }
    public Guid? ChildId { get; set; }
    public string StudentName { get; set; } = string.Empty;
    public bool IsChild { get; set; }
    
    public List<JournalCellDto> Cells { get; set; } = new();
    
    public decimal? AverageGrade { get; set; }
    public int AttendancePercent { get; set; }
    public int TotalLessons { get; set; }
    public int AttendedLessons { get; set; }
}

// DTO для ячейки журнала (одно занятие)
public class JournalCellDto
{
    public Guid LessonId { get; set; }
    public DateTime Date { get; set; }
    public LessonType LessonType { get; set; }
    
    public AttendanceStatus? AttendanceStatus { get; set; }
    public decimal? Grade { get; set; }
    public string? GradeLetter { get; set; }
}
