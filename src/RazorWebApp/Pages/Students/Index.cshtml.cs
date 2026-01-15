using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.RazorPages;
using RazorWebApp.Models.Common;
using RazorWebApp.Models.Students;
using RazorWebApp.Services;

namespace RazorWebApp.Pages.Students;

[Authorize(Roles = "Admin")]
public class IndexModel : PageModel
{
    private readonly StudentsService _studentsService;

    public IndexModel(StudentsService studentsService)
    {
        _studentsService = studentsService;
    }

    public PagedResponse<StudentViewModel>? Students { get; set; }
    public string? Search { get; set; }
    public int? StatusFilter { get; set; }
    public int CurrentPage { get; set; } = 1;

    public async Task OnGetAsync(string? search, int? status, int currentPage = 1)
    {
        Search = search;
        StatusFilter = status;
        CurrentPage = currentPage;
        Students = await _studentsService.ListAsync(search, status, currentPage);
    }
}
