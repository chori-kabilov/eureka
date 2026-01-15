using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using RazorWebApp.Models.Courses;
using RazorWebApp.Services;

namespace RazorWebApp.Pages.Courses;

[Authorize(Roles = "Admin")]
public class EditModel : PageModel
{
    private readonly CoursesService _coursesService;

    public EditModel(CoursesService coursesService)
    {
        _coursesService = coursesService;
    }

    public CourseViewModel? Course { get; set; }

    [BindProperty]
    public string Name { get; set; } = string.Empty;

    [BindProperty]
    public string? Description { get; set; }

    [BindProperty]
    public decimal Price { get; set; }

    [BindProperty]
    public int DurationHours { get; set; }

    [BindProperty]
    public int MaxStudents { get; set; }

    public string? SuccessMessage { get; set; }
    public string? ErrorMessage { get; set; }

    public async Task<IActionResult> OnGetAsync(Guid id)
    {
        var response = await _coursesService.GetAsync(id);
        
        if (response?.Success != true || response.Data == null)
            return RedirectToPage("/Courses/Index");

        Course = response.Data;
        Name = Course.Name;
        Description = Course.Description;
        Price = Course.Price;
        DurationHours = Course.DurationHours;
        MaxStudents = Course.MaxStudents;
        
        return Page();
    }

    public async Task<IActionResult> OnPostAsync(Guid id)
    {
        var result = await _coursesService.UpdateAsync(id, Name, Description, Price, DurationHours, MaxStudents);

        if (result?.Success == true)
        {
            SuccessMessage = "Курс успешно обновлён";
            Course = result.Data;
            return Page();
        }

        ErrorMessage = result?.Error ?? "Ошибка при обновлении";
        
        var response = await _coursesService.GetAsync(id);
        Course = response?.Data;
        
        return Page();
    }
}
