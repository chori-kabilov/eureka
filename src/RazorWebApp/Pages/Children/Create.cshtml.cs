using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using RazorWebApp.Models.Parents;
using RazorWebApp.Services;

namespace RazorWebApp.Pages.Children;

[Authorize(Roles = "Admin")]
public class CreateModel : PageModel
{
    private readonly ChildrenService _childrenService;
    private readonly ParentsService _parentsService;

    public CreateModel(ChildrenService childrenService, ParentsService parentsService)
    {
        _childrenService = childrenService;
        _parentsService = parentsService;
    }

    [BindProperty]
    public string FullName { get; set; } = string.Empty;

    [BindProperty]
    public Guid ParentId { get; set; }

    [BindProperty]
    public DateTime? BirthDate { get; set; }

    [BindProperty]
    public string? Notes { get; set; }

    public string? ErrorMessage { get; set; }
    public List<ParentViewModel> AvailableParents { get; set; } = new();

    public async Task OnGetAsync()
    {
        await LoadParents();
    }

    public async Task<IActionResult> OnPostAsync()
    {
        if (string.IsNullOrWhiteSpace(FullName))
        {
            ErrorMessage = "Введите ФИО ребёнка";
            await LoadParents();
            return Page();
        }

        if (ParentId == Guid.Empty)
        {
            ErrorMessage = "Выберите родителя";
            await LoadParents();
            return Page();
        }

        var result = await _childrenService.CreateAsync(ParentId, FullName, BirthDate, Notes);

        if (result?.Success == true)
            return RedirectToPage("/Children/Index");

        ErrorMessage = result?.Error ?? "Не удалось создать ребёнка";
        await LoadParents();
        return Page();
    }

    private async Task LoadParents()
    {
        var parents = await _parentsService.ListAsync(null, 1);
        if (parents?.Items != null)
        {
            AvailableParents = parents.Items;
        }
    }
}
