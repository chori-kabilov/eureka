using System.ComponentModel.DataAnnotations;

namespace WebApi.Contracts.Auth;

// API контракт для регистрации
public class RegisterApiRequest
{
    [Required(ErrorMessage = "Телефон обязателен")]
    [Phone(ErrorMessage = "Некорректный формат телефона")]
    public string Phone { get; set; } = string.Empty;

    [Required(ErrorMessage = "ФИО обязательно")]
    [MaxLength(200, ErrorMessage = "ФИО не более 200 символов")]
    public string FullName { get; set; } = string.Empty;

    [Required(ErrorMessage = "Пароль обязателен")]
    [MinLength(6, ErrorMessage = "Пароль минимум 6 символов")]
    public string Password { get; set; } = string.Empty;
}
