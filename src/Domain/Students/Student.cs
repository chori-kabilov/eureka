using Domain.Common;
using Domain.Users;

namespace Domain.Students;

// Профиль студента (расширение User)
public class Student : BaseEntity
{
    public Guid UserId { get; set; }
    public User User { get; set; } = null!;
    
    public StudentStatus Status { get; set; }
    public string? Notes { get; set; }
    
    // Если был ребёнком — ссылка на исходные данные
    public Guid? ChildId { get; set; }
    public Child? Child { get; set; }
}
