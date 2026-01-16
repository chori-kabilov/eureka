using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using RazorWebApp.Services;

namespace RazorWebApp.Pages.Grading;

[Authorize(Roles = "Admin")]
public class IndexModel : PageModel
{
    private readonly GradingService _gradingService;

    public IndexModel(GradingService gradingService)
    {
        _gradingService = gradingService;
    }

    public List<GradingSystemViewModel> GradingSystems { get; set; } = new();

    public async Task OnGetAsync()
    {
        GradingSystems = await _gradingService.ListAsync();
    }

    public async Task<IActionResult> OnPostAsync(string name, int type, decimal minScore, decimal maxScore, decimal passingScore, bool isDefault = false)
    {
        if (!string.IsNullOrWhiteSpace(name))
        {
            await _gradingService.CreateAsync(name, type, minScore, maxScore, passingScore, isDefault);
        }
        return RedirectToPage();
    }
}
