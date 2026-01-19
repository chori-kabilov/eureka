using Domain.Common;
using Domain.Parents;
using Domain.Students;

namespace Domain.Users;

// Ребёнок (без аккаунта, привязан к родителю)
public class Child : BaseEntity
{
    public Guid ParentId { get; set; }
    public Parent Parent { get; set; } = null!;
    
    public string FullName { get; set; } = string.Empty;
    public DateTime? BirthDate { get; set; }
    public Gender? Gender { get; set; }
    public EducationLevel? EducationLevel { get; set; }
    public StudentStatus Status { get; set; }
    public string? Notes { get; set; }
    
    // Когда ребёнок получает свой аккаунт
    public Guid? LinkedStudentId { get; set; }
    public Student? LinkedStudent { get; set; }
}
