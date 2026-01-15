namespace Application.Modules.Users.Dtos;

// DTO пользователя для ответа
public class UserDto
{
    public Guid Id { get; set; }
    public string Phone { get; set; } = string.Empty;
    public string FullName { get; set; } = string.Empty;
    public bool IsAdmin { get; set; }
    public bool IsStudent { get; set; }
    public bool IsTeacher { get; set; }
    public bool IsParent { get; set; }
    public DateTime CreatedAt { get; set; }
}

// Детальный DTO пользователя
public class UserDetailDto : UserDto
{
    public DateTime? UpdatedAt { get; set; }
    public bool IsDeleted { get; set; }
}
