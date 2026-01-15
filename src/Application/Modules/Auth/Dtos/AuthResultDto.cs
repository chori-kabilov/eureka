namespace Application.Modules.Auth.Dtos;

// Результат авторизации
public class AuthResultDto
{
    public Guid UserId { get; set; }
    public string FullName { get; set; } = string.Empty;
    public string Phone { get; set; } = string.Empty;
    public bool IsAdmin { get; set; }
    public bool IsStudent { get; set; }
    public bool IsTeacher { get; set; }
    public bool IsParent { get; set; }
    public string Token { get; set; } = string.Empty;
}
