using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace RazorWebApp.Pages;

[ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
[IgnoreAntiforgeryToken]
public class ErrorModel : PageModel
{
    public string Title { get; set; } = "Ошибка";
    public string Message { get; set; } = "Что-то пошло не так";
    public string Icon { get; set; } = "bi-exclamation-triangle";
    public string IconColor { get; set; } = "text-warning";

    public void OnGet(int? statusCode)
    {
        switch (statusCode)
        {
            case 401:
                Title = "Требуется авторизация";
                Message = "Войдите в систему для доступа к этой странице";
                Icon = "bi-lock";
                IconColor = "text-primary";
                break;
            case 403:
                Title = "Доступ запрещён";
                Message = "У вас нет прав для просмотра этой страницы";
                Icon = "bi-shield-x";
                IconColor = "text-danger";
                break;
            case 404:
                Title = "Страница не найдена";
                Message = "Запрашиваемая страница не существует";
                Icon = "bi-search";
                IconColor = "text-muted";
                break;
            case 500:
            default:
                Title = "Ошибка сервера";
                Message = "Произошла внутренняя ошибка. Попробуйте позже";
                Icon = "bi-exclamation-triangle";
                IconColor = "text-warning";
                break;
        }
    }
}
