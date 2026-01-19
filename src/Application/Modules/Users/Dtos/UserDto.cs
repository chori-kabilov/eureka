using Domain.Users;

namespace Application.Modules.Users.Dtos;

// DTO пользователя для ответа
public class UserDto
{
    public Guid Id { get; set; }
    public string Phone { get; set; } = string.Empty;
    public string FullName { get; set; } = string.Empty;
    public DateTime? BirthDate { get; set; }
    
    // Новые поля
    public Gender? Gender { get; set; }
    public string? Nationality { get; set; }
    public string? Address { get; set; }
    public string? ProfilePhotoUrl { get; set; }
    
    public bool IsAdmin { get; set; }
    public bool IsStudent { get; set; }
    public bool IsTeacher { get; set; }
    public bool IsParent { get; set; }
    public DateTime CreatedAt { get; set; }
}
