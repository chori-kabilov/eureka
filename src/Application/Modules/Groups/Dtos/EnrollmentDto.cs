using Domain.Groups;
using Domain.Schedule;
using Domain.Journal;
using Domain.Grading;

namespace Application.Modules.Groups.Dtos;

// DTO зачисления
public class EnrollmentDto
{
    public Guid Id { get; set; }
    public Guid GroupId { get; set; }
    
    public Guid? StudentId { get; set; }
    public string? StudentName { get; set; }
    public string? StudentPhone { get; set; }
    
    public Guid? ChildId { get; set; }
    public string? ChildName { get; set; }
    public string? ParentName { get; set; }
    
    public DateTime EnrolledAt { get; set; }
    public DateTime? LeftAt { get; set; }
    
    public EnrollmentStatus Status { get; set; }
    public string? Notes { get; set; }
    
    public string DisplayName => StudentName ?? ChildName ?? "Неизвестно";
    public bool IsChild => ChildId.HasValue;
}
