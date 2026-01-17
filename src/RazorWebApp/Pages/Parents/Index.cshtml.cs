using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using RazorWebApp.Models.Common;
using RazorWebApp.Models.Parents;
using RazorWebApp.Services;

namespace RazorWebApp.Pages.Parents;

[Authorize(Roles = "Admin")]
public class IndexModel : PageModel
{
    private readonly ParentsService _parentsService;

    public IndexModel(ParentsService parentsService)
    {
        _parentsService = parentsService;
    }

    public PagedResponse<ParentViewModel>? Parents { get; set; }
    public string? Search { get; set; }
    public int CurrentPage { get; set; } = 1;
    public int PageSize { get; set; } = 12;

    public async Task OnGetAsync(string? search, int currentPage = 1, int pageSize = 12)
    {
        Search = search;
        CurrentPage = currentPage;
        PageSize = pageSize;
        Parents = await _parentsService.ListAsync(search, currentPage, pageSize);
    }

    public async Task<IActionResult> OnPostDeleteAsync(Guid id)
    {
        await _parentsService.DeleteAsync(id);
        return RedirectToPage();
    }
}
