namespace Application.Modules.Auth.UseCases.Register;

// Запрос на регистрацию
public class RegisterRequest
{
    public string Phone { get; set; } = string.Empty;
    public string FullName { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
}
