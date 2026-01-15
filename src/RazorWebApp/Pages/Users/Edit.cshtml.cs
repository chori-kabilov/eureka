using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using RazorWebApp.Models.Users;
using RazorWebApp.Services;

namespace RazorWebApp.Pages.Users;

[Authorize(Roles = "Admin")]
public class EditModel : PageModel
{
    private readonly UsersService _usersService;

    public EditModel(UsersService usersService)
    {
        _usersService = usersService;
    }

    public UserViewModel? UserData { get; set; }

    [BindProperty]
    public bool IsAdmin { get; set; }

    public string? ErrorMessage { get; set; }
    public string? SuccessMessage { get; set; }

    public async Task<IActionResult> OnGetAsync(Guid id)
    {
        var response = await _usersService.GetAsync(id);
        
        if (response?.Success != true || response.Data == null)
        {
            return RedirectToPage("/Users/Index");
        }

        UserData = response.Data;
        IsAdmin = UserData.IsAdmin;
        return Page();
    }

    public async Task<IActionResult> OnPostAsync(Guid id)
    {
        var response = await _usersService.UpdateAdminAsync(id, IsAdmin);

        if (response?.Success == true)
        {
            SuccessMessage = "Статус успешно изменён";
            UserData = response.Data;
            return Page();
        }

        ErrorMessage = response?.Error ?? "Ошибка при изменении статуса";
        
        // Перезагрузить данные пользователя
        var userResponse = await _usersService.GetAsync(id);
        UserData = userResponse?.Data;
        
        return Page();
    }
}
