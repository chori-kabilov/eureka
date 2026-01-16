using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using RazorWebApp.Services;

namespace RazorWebApp.Pages.Lessons;

[Authorize(Roles = "Admin")]
public class AttendanceModel : PageModel
{
    private readonly LessonsService _lessonsService;

    public AttendanceModel(LessonsService lessonsService)
    {
        _lessonsService = lessonsService;
    }

    public LessonDetailViewModel? Lesson { get; set; }
    public List<StudentAttendanceItem> Students { get; set; } = new();
    public string? SuccessMessage { get; set; }

    public async Task<IActionResult> OnGetAsync(Guid lessonId)
    {
        var response = await _lessonsService.GetLessonWithAttendanceAsync(lessonId);
        if (response == null)
            return RedirectToPage("/Groups/Index");

        Lesson = response.Lesson;
        Students = response.Students;
        
        return Page();
    }

    public async Task<IActionResult> OnPostAsync(Guid lessonId, List<AttendanceItem> items)
    {
        var result = await _lessonsService.SaveAttendanceAsync(lessonId, items);
        
        if (result)
        {
            return RedirectToPage(new { lessonId, success = true });
        }
        
        return RedirectToPage(new { lessonId });
    }
}

// ViewModels
public class LessonDetailViewModel
{
    public Guid Id { get; set; }
    public Guid GroupId { get; set; }
    public string GroupName { get; set; } = string.Empty;
    public DateTime Date { get; set; }
    public TimeSpan StartTime { get; set; }
    public TimeSpan EndTime { get; set; }
}

public class StudentAttendanceItem
{
    public Guid? StudentId { get; set; }
    public Guid? ChildId { get; set; }
    public string Name { get; set; } = string.Empty;
    public bool IsChild { get; set; }
    public int? Status { get; set; }
}

public class AttendanceItem
{
    public Guid? StudentId { get; set; }
    public Guid? ChildId { get; set; }
    public int Status { get; set; }
}

public class LessonAttendanceResponse
{
    public LessonDetailViewModel Lesson { get; set; } = null!;
    public List<StudentAttendanceItem> Students { get; set; } = new();
}
