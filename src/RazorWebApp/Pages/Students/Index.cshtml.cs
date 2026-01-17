using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using RazorWebApp.Models.Common;
using RazorWebApp.Models.Students;
using RazorWebApp.Models.Users;
using RazorWebApp.Services;

namespace RazorWebApp.Pages.Students;

[Authorize(Roles = "Admin")]
public class IndexModel : PageModel
{
    private readonly StudentsService _studentsService;
    private readonly UsersService _usersService;

    public IndexModel(StudentsService studentsService, UsersService usersService)
    {
        _studentsService = studentsService;
        _usersService = usersService;
    }

    public PagedResponse<StudentViewModel>? Students { get; set; }
    public List<UserViewModel> AvailableUsers { get; set; } = new();
    public string? Search { get; set; }
    public int? StatusFilter { get; set; }
    public int CurrentPage { get; set; } = 1;
    public int PageSize { get; set; } = 12;

    public async Task OnGetAsync(string? search, int? status, int currentPage = 1, int pageSize = 12)
    {
        Search = search;
        StatusFilter = status;
        CurrentPage = currentPage;
        PageSize = pageSize;
        Students = await _studentsService.ListAsync(search, status, currentPage, pageSize);
        
        // Загружаем пользователей без роли студента
        var usersResponse = await _usersService.ListAsync(hasStudent: false, pageSize: 100);
        AvailableUsers = usersResponse?.Items ?? new List<UserViewModel>();
    }

    public async Task<IActionResult> OnPostDeleteAsync(Guid id)
    {
        await _studentsService.DeleteAsync(id);
        return RedirectToPage();
    }

    public async Task<IActionResult> OnPostUpdateAsync(
        Guid id, 
        string fullName, 
        string phone, 
        int status, 
        string? notes)
    {
        await _studentsService.UpdateAsync(id, fullName, phone, status, notes);
        return RedirectToPage();
    }

    public async Task<IActionResult> OnPostCreateAsync(Guid userId, string? notes)
    {
        await _studentsService.CreateAsync(userId, notes);
        return RedirectToPage();
    }
}
