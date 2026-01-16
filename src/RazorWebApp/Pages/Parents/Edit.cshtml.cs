using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using RazorWebApp.Models.Parents;
using RazorWebApp.Services;

namespace RazorWebApp.Pages.Parents;

[Authorize(Roles = "Admin")]
public class EditModel : PageModel
{
    private readonly ParentsService _parentsService;

    public EditModel(ParentsService parentsService)
    {
        _parentsService = parentsService;
    }

    public ParentViewModel? Parent { get; set; }

    [BindProperty]
    public string? Notes { get; set; }

    public async Task<IActionResult> OnGetAsync(Guid id)
    {
        Parent = await _parentsService.GetAsync(id);
        if (Parent == null)
            return RedirectToPage("/Parents/Index");
        
        return Page();
    }

    public async Task<IActionResult> OnPostAsync(Guid id)
    {
        var request = new UpdateParentRequest { Notes = Notes };
        var result = await _parentsService.UpdateAsync(id, request);
        
        if (result)
            return RedirectToPage("/Parents/Index");
            
        return await OnGetAsync(id);
    }
}
