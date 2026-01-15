using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using RazorWebApp.Models.Users;
using RazorWebApp.Services;

namespace RazorWebApp.Pages.Teachers;

[Authorize(Roles = "Admin")]
public class CreateModel : PageModel
{
    private readonly TeachersService _teachersService;
    private readonly UsersService _usersService;

    public CreateModel(TeachersService teachersService, UsersService usersService)
    {
        _teachersService = teachersService;
        _usersService = usersService;
    }

    [BindProperty]
    public Guid UserId { get; set; }

    [BindProperty]
    public string? Specialization { get; set; }

    [BindProperty]
    public int PaymentType { get; set; }

    [BindProperty]
    public decimal? HourlyRate { get; set; }

    [BindProperty]
    public string? Bio { get; set; }

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

        var result = await _teachersService.CreateAsync(UserId, Specialization, PaymentType, HourlyRate, Bio);

        if (result?.Success == true)
            return RedirectToPage("/Teachers/Index");

        ErrorMessage = result?.Error ?? "Не удалось создать учителя";
        await LoadAvailableUsers();
        return Page();
    }

    private async Task LoadAvailableUsers()
    {
        // Загружаем пользователей без профиля учителя
        var users = await _usersService.ListAsync(null, null, 1);
        if (users?.Items != null)
        {
            AvailableUsers = users.Items.Where(u => !u.IsTeacher).ToList();
        }
    }
}
