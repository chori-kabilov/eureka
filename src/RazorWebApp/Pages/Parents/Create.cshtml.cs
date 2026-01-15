using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using RazorWebApp.Models.Users;
using RazorWebApp.Services;

namespace RazorWebApp.Pages.Parents;

[Authorize(Roles = "Admin")]
public class CreateModel : PageModel
{
    private readonly ParentsService _parentsService;
    private readonly UsersService _usersService;

    public CreateModel(ParentsService parentsService, UsersService usersService)
    {
        _parentsService = parentsService;
        _usersService = usersService;
    }

    [BindProperty]
    public Guid UserId { get; set; }

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

        var result = await _parentsService.CreateAsync(UserId);

        if (result?.Success == true)
            return RedirectToPage("/Parents/Index");

        ErrorMessage = result?.Error ?? "Не удалось создать родителя";
        await LoadAvailableUsers();
        return Page();
    }

    private async Task LoadAvailableUsers()
    {
        var users = await _usersService.ListAsync(null, null, 1);
        if (users?.Items != null)
        {
            AvailableUsers = users.Items.Where(u => !u.IsParent).ToList();
        }
    }
}
