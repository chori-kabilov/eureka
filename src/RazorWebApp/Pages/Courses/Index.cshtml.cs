using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using RazorWebApp.Models.Common;
using RazorWebApp.Models.Courses;
using RazorWebApp.Services;

namespace RazorWebApp.Pages.Courses;

[Authorize(Roles = "Admin")]
public class IndexModel : PageModel
{
    private readonly CoursesService _coursesService;

    public IndexModel(CoursesService coursesService)
    {
        _coursesService = coursesService;
    }

    public PagedResponse<CourseViewModel>? Courses { get; set; }
    public string? Search { get; set; }
    public bool ShowArchived { get; set; }
    public int CurrentPage { get; set; } = 1;
    public int PageSize { get; set; } = 12;

    public async Task OnGetAsync(string? search, bool showArchived = false, int currentPage = 1, int pageSize = 12)
    {
        Search = search;
        ShowArchived = showArchived;
        CurrentPage = currentPage;
        PageSize = pageSize;
        Courses = await _coursesService.ListAsync(search, showArchived, currentPage, pageSize);
    }

    public async Task<IActionResult> OnPostDeleteAsync(Guid id)
    {
        await _coursesService.DeleteAsync(id);
        return RedirectToPage();
    }
}
