using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using RazorWebApp.Models.Users;
using RazorWebApp.Services;

namespace RazorWebApp.Pages.Users;

[Authorize(Roles = "Admin")]
public class IndexModel : PageModel
{
    private readonly UsersService _usersService;

    public IndexModel(UsersService usersService)
    {
        _usersService = usersService;
    }

    public UsersPagedResponse? Users { get; set; }
    
    [BindProperty(SupportsGet = true)]
    public string? Search { get; set; }
    
    [BindProperty(SupportsGet = true)]
    public int CurrentPage { get; set; } = 1;
    
    [BindProperty(SupportsGet = true)]
    public int PageSize { get; set; } = 12;

    public async Task OnGetAsync()
    {
        Users = await _usersService.ListAsync(Search, null, CurrentPage, PageSize);
    }

    public async Task<IActionResult> OnPostDeleteAsync(Guid id)
    {
        await _usersService.DeleteAsync(id);
        return RedirectToPage();
    }
}
