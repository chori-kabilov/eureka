using System.ComponentModel.DataAnnotations;

namespace RazorWebApp.Models.Auth;

// Запрос на регистрацию
public class RegisterRequest
{
    [Required(ErrorMessage = "Телефон обязателен")]
    [Display(Name = "Телефон")]
    public string Phone { get; set; } = string.Empty;

    [Required(ErrorMessage = "ФИО обязательно")]
    [Display(Name = "ФИО")]
    public string FullName { get; set; } = string.Empty;

    [Required(ErrorMessage = "Пароль обязателен")]
    [MinLength(6, ErrorMessage = "Минимум 6 символов")]
    [Display(Name = "Пароль")]
    [DataType(DataType.Password)]
    public string Password { get; set; } = string.Empty;
}

// Запрос на вход
public class LoginRequest
{
    [Required(ErrorMessage = "Телефон обязателен")]
    [Display(Name = "Телефон")]
    public string Phone { get; set; } = string.Empty;

    [Required(ErrorMessage = "Пароль обязателен")]
    [Display(Name = "Пароль")]
    [DataType(DataType.Password)]
    public string Password { get; set; } = string.Empty;
}

// Результат авторизации
public class AuthResult
{
    public Guid UserId { get; set; }
    public string FullName { get; set; } = string.Empty;
    public string Phone { get; set; } = string.Empty;
    public int Role { get; set; }
    public string Token { get; set; } = string.Empty;

    // Название роли для отображения
    public string RoleName => Role switch
    {
        0 => "User",
        1 => "Admin",
        2 => "Teacher",
        3 => "Student",
        4 => "Parent",
        _ => "User"
    };
}
