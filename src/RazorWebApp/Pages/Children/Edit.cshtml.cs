using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using RazorWebApp.Models.Children;
using RazorWebApp.Services;

namespace RazorWebApp.Pages.Children;

[Authorize(Roles = "Admin")]
public class EditModel : PageModel
{
    private readonly ChildrenService _childrenService;

    public EditModel(ChildrenService childrenService)
    {
        _childrenService = childrenService;
    }

    public ChildViewModel? Child { get; set; }

    [BindProperty]
    public int Status { get; set; }

    [BindProperty]
    public string? Notes { get; set; }

    public string? SuccessMessage { get; set; }
    public string? ErrorMessage { get; set; }

    public async Task<IActionResult> OnGetAsync(Guid id)
    {
        var response = await _childrenService.GetAsync(id);
        
        if (response?.Success != true || response.Data == null)
            return RedirectToPage("/Children/Index");

        Child = response.Data;
        Status = Child.Status;
        Notes = Child.Notes;
        
        return Page();
    }

    public async Task<IActionResult> OnPostAsync(Guid id)
    {
        var result = await _childrenService.UpdateAsync(id, Status, Notes);

        if (result?.Success == true)
        {
            SuccessMessage = "Данные успешно обновлены";
            Child = result.Data;
            return Page();
        }

        ErrorMessage = result?.Error ?? "Ошибка при обновлении";
        
        var response = await _childrenService.GetAsync(id);
        Child = response?.Data;
        
        return Page();
    }
}
