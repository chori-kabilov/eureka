using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.RazorPages;
using RazorWebApp.Models.Common;
using RazorWebApp.Models.Children;
using RazorWebApp.Services;

namespace RazorWebApp.Pages.Children;

[Authorize(Roles = "Admin")]
public class IndexModel : PageModel
{
    private readonly ChildrenService _childrenService;

    public IndexModel(ChildrenService childrenService)
    {
        _childrenService = childrenService;
    }

    public PagedResponse<ChildViewModel>? Children { get; set; }
    public string? Search { get; set; }
    public int CurrentPage { get; set; } = 1;

    public async Task OnGetAsync(string? search, int currentPage = 1)
    {
        Search = search;
        CurrentPage = currentPage;
        Children = await _childrenService.ListAsync(search, null, currentPage);
    }
}
