using Domain.Common;
using Domain.Enums;
using Domain.Students;
using Domain.Users;

namespace Domain.Groups;

// Зачисление ученика в группу
public class GroupEnrollment : BaseEntity
{
    public Guid GroupId { get; set; }
    public Group Group { get; set; } = null!;
    
    // Взрослый студент
    public Guid? StudentId { get; set; }
    public Student? Student { get; set; }
    
    // Ребёнок
    public Guid? ChildId { get; set; }
    public Child? Child { get; set; }
    
    public DateTime EnrolledAt { get; set; }
    public DateTime? LeftAt { get; set; }
    
    public EnrollmentStatus Status { get; set; } = EnrollmentStatus.Active;
    
    // История переводов
    public Guid? TransferredFromGroupId { get; set; }
    public Group? TransferredFromGroup { get; set; }
    
    public Guid? TransferredToGroupId { get; set; }
    public Group? TransferredToGroup { get; set; }
    
    public string? Notes { get; set; }
}
