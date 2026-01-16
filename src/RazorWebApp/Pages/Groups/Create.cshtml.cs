using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using RazorWebApp.Models.Courses;
using RazorWebApp.Models.Teachers;
using RazorWebApp.Services;

namespace RazorWebApp.Pages.Groups;

[Authorize(Roles = "Admin")]
public class CreateModel : PageModel
{
    private readonly GroupsService _groupsService;
    private readonly CoursesService _coursesService;
    private readonly TeachersService _teachersService;

    public CreateModel(GroupsService groupsService, CoursesService coursesService, TeachersService teachersService)
    {
        _groupsService = groupsService;
        _coursesService = coursesService;
        _teachersService = teachersService;
    }

    [BindProperty]
    public string Name { get; set; } = string.Empty;

    [BindProperty]
    public string? Code { get; set; }

    [BindProperty]
    public Guid CourseId { get; set; }

    [BindProperty]
    public Guid TeacherId { get; set; }

    [BindProperty]
    public DateTime StartDate { get; set; } = DateTime.Today;

    [BindProperty]
    public DateTime? EndDate { get; set; }

    [BindProperty]
    public int MaxStudents { get; set; } = 15;

    [BindProperty]
    public string? Notes { get; set; }

    public List<CourseViewModel> Courses { get; set; } = new();
    public List<TeacherViewModel> Teachers { get; set; } = new();
    public string? ErrorMessage { get; set; }

    public async Task OnGetAsync()
    {
        await LoadDataAsync();
    }

    public async Task<IActionResult> OnPostAsync()
    {
        if (string.IsNullOrWhiteSpace(Name))
        {
            ErrorMessage = "Введите название группы";
            await LoadDataAsync();
            return Page();
        }

        var result = await _groupsService.CreateAsync(
            Name, Code, CourseId, TeacherId, StartDate, EndDate, MaxStudents, Notes);

        if (result?.Success == true)
            return RedirectToPage("/Groups/Details", new { id = result.Data?.Id });

        ErrorMessage = result?.Error ?? "Не удалось создать группу";
        await LoadDataAsync();
        return Page();
    }

    private async Task LoadDataAsync()
    {
        var coursesResult = await _coursesService.ListAsync();
        Courses = coursesResult?.Items?.ToList() ?? new List<CourseViewModel>();

        var teachersResult = await _teachersService.ListAsync();
        Teachers = teachersResult?.Items?.ToList() ?? new List<TeacherViewModel>();
    }
}
