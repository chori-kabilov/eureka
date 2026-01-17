using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using RazorWebApp.Models.Users;
using RazorWebApp.Services;

namespace RazorWebApp.Pages.Students;

[Authorize(Roles = "Admin")]
public class CreateModel : PageModel
{
    private readonly StudentsService _studentsService;
    private readonly UsersService _usersService;

    public CreateModel(StudentsService studentsService, UsersService usersService)
    {
        _studentsService = studentsService;
        _usersService = usersService;
    }

    [BindProperty]
    public Guid UserId { get; set; }

    [BindProperty]
    public string? Notes { get; set; }

    public string? ErrorMessage { get; set; }
    public List<UserViewModel> AvailableUsers { get; set; } = new();

    public async Task OnGetAsync()
    {
        await LoadAvailableUsers();
    }

    public async Task<IActionResult> OnPostAsync()
    {
        if (UserId == Guid.Empty)
        {
            ErrorMessage = "Выберите пользователя";
            await LoadAvailableUsers();
            return Page();
        }

        var result = await _studentsService.CreateAsync(UserId, Notes);

        if (result?.Success == true)
            return RedirectToPage("/Students/Index");

        ErrorMessage = result?.Error ?? "Не удалось создать студента";
        await LoadAvailableUsers();
        return Page();
    }

    private async Task LoadAvailableUsers()
    {
        // Загружаем пользователей без студенческого профиля
        var users = await _usersService.ListAsync(hasStudent: false, pageSize: 100);
        AvailableUsers = users?.Items ?? new List<UserViewModel>();
    }
}
