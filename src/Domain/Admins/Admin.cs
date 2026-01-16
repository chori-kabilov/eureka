using Domain.Common;
using Domain.Users;

namespace Domain.Admins;

// Профиль администратора (расширение User)
public class Admin : BaseEntity
{
    public Guid UserId { get; set; }
    public User User { get; set; } = null!;
    
    public AdminAccessLevel AccessLevel { get; set; }
    public string? Department { get; set; }
    public string? Notes { get; set; }
}
