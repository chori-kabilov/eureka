using Domain.Groups;
using Domain.Schedule;
using Domain.Journal;
using Domain.Grading;

namespace Application.Modules.Groups.Dtos;

// DTO группы для списка
public class GroupDto
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string? Code { get; set; }
    
    public Guid CourseId { get; set; }
    public string CourseName { get; set; } = string.Empty;
    
    public Guid ResponsibleTeacherId { get; set; }
    public string ResponsibleTeacherName { get; set; } = string.Empty;
    
    public DateTime StartDate { get; set; }
    public DateTime? EndDate { get; set; }
    
    public int MaxStudents { get; set; }
    public int CurrentStudents { get; set; }
    
    public GroupStatus Status { get; set; }
    public DateTime CreatedAt { get; set; }
}

