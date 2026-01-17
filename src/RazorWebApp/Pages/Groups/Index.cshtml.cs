using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using RazorWebApp.Models.Common;
using RazorWebApp.Models.Groups;
using RazorWebApp.Services;

namespace RazorWebApp.Pages.Groups;

[Authorize(Roles = "Admin")]
public class IndexModel : PageModel
{
    private readonly GroupsService _groupsService;

    public IndexModel(GroupsService groupsService)
    {
        _groupsService = groupsService;
    }

    public PagedResponse<GroupViewModel>? Groups { get; set; }
    public string? Search { get; set; }
    public int? Status { get; set; }
    public int CurrentPage { get; set; } = 1;
    public int PageSize { get; set; } = 12;

    public async Task OnGetAsync(string? search, int? status, int currentPage = 1, int pageSize = 12)
    {
        Search = search;
        Status = status;
        CurrentPage = currentPage;
        PageSize = pageSize;
        Groups = await _groupsService.ListAsync(search, null, status, currentPage, pageSize);
    }
}
