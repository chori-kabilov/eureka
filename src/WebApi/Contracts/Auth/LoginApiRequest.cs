using System.ComponentModel.DataAnnotations;

namespace WebApi.Contracts.Auth;

// API контракт для входа
public class LoginApiRequest
{
    [Required(ErrorMessage = "Телефон обязателен")]
    public string Phone { get; set; } = string.Empty;

    [Required(ErrorMessage = "Пароль обязателен")]
    public string Password { get; set; } = string.Empty;
}
