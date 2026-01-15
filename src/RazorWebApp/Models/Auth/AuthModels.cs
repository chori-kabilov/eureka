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
    public bool IsAdmin { get; set; }
    public bool IsStudent { get; set; }
    public bool IsTeacher { get; set; }
    public bool IsParent { get; set; }
    public string Token { get; set; } = string.Empty;

    // Название роли для отображения
    public string RoleName
    {
        get
        {
            var roles = new List<string>();
            if (IsAdmin) roles.Add("Admin");
            if (IsTeacher) roles.Add("Teacher");
            if (IsStudent) roles.Add("Student");
            if (IsParent) roles.Add("Parent");
            return roles.Count > 0 ? string.Join(", ", roles) : "User";
        }
    }
}
