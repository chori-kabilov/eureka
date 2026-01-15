using Domain.Common;

namespace Domain.Users;

// Сущность пользователя
public class User : BaseEntity
{
    public string Phone { get; set; } = string.Empty;
    public string FullName { get; set; } = string.Empty;
    public string PasswordHash { get; set; } = string.Empty;
    public Role Role { get; set; } = Role.User;
}
