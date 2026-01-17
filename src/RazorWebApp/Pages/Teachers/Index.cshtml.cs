using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using RazorWebApp.Models.Common;
using RazorWebApp.Models.Teachers;
using RazorWebApp.Models.Users;
using RazorWebApp.Services;

namespace RazorWebApp.Pages.Teachers;

[Authorize(Roles = "Admin")]
public class IndexModel : PageModel
{
    private readonly TeachersService _teachersService;
    private readonly UsersService _usersService;

    public IndexModel(TeachersService teachersService, UsersService usersService)
    {
        _teachersService = teachersService;
        _usersService = usersService;
    }

    public PagedResponse<TeacherViewModel>? Teachers { get; set; }
    public List<UserViewModel> AvailableUsers { get; set; } = new();
    public string? Search { get; set; }
    public int CurrentPage { get; set; } = 1;
    public int PageSize { get; set; } = 12;

    public async Task OnGetAsync(string? search, int currentPage = 1, int pageSize = 12)
    {
        Search = search;
        CurrentPage = currentPage;
        PageSize = pageSize;
        Teachers = await _teachersService.ListAsync(search, currentPage, pageSize);
        
        var usersResponse = await _usersService.ListAsync(pageSize: 100);
        AvailableUsers = usersResponse?.Items?.Where(u => !u.IsTeacher).ToList() ?? new List<UserViewModel>();
    }

    public async Task<IActionResult> OnPostDeleteAsync(Guid id)
    {
        await _teachersService.DeleteAsync(id);
        return RedirectToPage();
    }

    public async Task<IActionResult> OnPostUpdateAsync(
        Guid id, 
        string fullName, 
        string phone, 
        int status,
        string? subjects,
        int paymentType,
        decimal? hourlyRate,
        string? bio)
    {
        var subjectsList = string.IsNullOrWhiteSpace(subjects) 
            ? new List<string>() 
            : subjects.Split(',', StringSplitOptions.RemoveEmptyEntries).Select(s => s.Trim()).ToList();
        await _teachersService.UpdateAsync(id, fullName, phone, status, subjectsList, paymentType, hourlyRate, bio);
        return RedirectToPage();
    }

    public async Task<IActionResult> OnPostCreateAsync(
        Guid userId, 
        string? subjects,
        int paymentType,
        decimal? hourlyRate,
        string? bio)
    {
        var subjectsList = string.IsNullOrWhiteSpace(subjects) 
            ? new List<string>() 
            : subjects.Split(',', StringSplitOptions.RemoveEmptyEntries).Select(s => s.Trim()).ToList();
        await _teachersService.CreateAsync(userId, subjectsList, paymentType, hourlyRate, bio);
        return RedirectToPage();
    }
}
