using Domain.Common;
using Domain.Users;

namespace Domain.Admins;

// Уровень доступа администратора
public enum AdminAccessLevel
{
    Limited = 0,    // Ограниченный
    Full = 1,       // Полный
    Super = 2       // Суперадмин
}

// Профиль администратора (расширение User)
public class Admin : BaseEntity
{
    public Guid UserId { get; set; }
    public User User { get; set; } = null!;
    
    public AdminAccessLevel AccessLevel { get; set; }
    public string? Department { get; set; }
    public string? Notes { get; set; }
}
