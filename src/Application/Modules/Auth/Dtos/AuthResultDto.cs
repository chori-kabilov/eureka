using Domain.Users;

namespace Application.Modules.Auth.Dtos;

// Результат авторизации
public class AuthResultDto
{
    public Guid UserId { get; set; }
    public string FullName { get; set; } = string.Empty;
    public string Phone { get; set; } = string.Empty;
    public Role Role { get; set; }
    public string Token { get; set; } = string.Empty;
}
