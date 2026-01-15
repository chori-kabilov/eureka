using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using RazorWebApp.Services;

namespace RazorWebApp.Pages.Courses;

[Authorize(Roles = "Admin")]
public class CreateModel : PageModel
{
    private readonly CoursesService _coursesService;

    public CreateModel(CoursesService coursesService)
    {
        _coursesService = coursesService;
    }

    [BindProperty]
    public string Name { get; set; } = string.Empty;

    [BindProperty]
    public string? Description { get; set; }

    [BindProperty]
    public decimal Price { get; set; } = 100000;

    [BindProperty]
    public int DurationHours { get; set; } = 10;

    [BindProperty]
    public int MaxStudents { get; set; } = 15;

    public string? ErrorMessage { get; set; }

    public void OnGet()
    {
    }

    public async Task<IActionResult> OnPostAsync()
    {
        if (string.IsNullOrWhiteSpace(Name))
        {
            ErrorMessage = "Введите название курса";
            return Page();
        }

        var result = await _coursesService.CreateAsync(Name, Description, Price, DurationHours, MaxStudents);

        if (result?.Success == true)
            return RedirectToPage("/Courses/Index");

        ErrorMessage = result?.Error ?? "Не удалось создать курс";
        return Page();
    }
}
