using Domain.Admins;
using Domain.Common;
using Domain.Parents;
using Domain.Students;
using Domain.Teachers;

namespace Domain.Users;

// Сущность пользователя (базовый аккаунт)
public class User : BaseEntity
{
    public string Phone { get; set; } = string.Empty;
    public string FullName { get; set; } = string.Empty;
    public string PasswordHash { get; set; } = string.Empty;
    
    // Профили (один User может иметь несколько)
    public Admin? AdminProfile { get; set; }
    public Student? StudentProfile { get; set; }
    public Teacher? TeacherProfile { get; set; }
    public Parent? ParentProfile { get; set; }
}
