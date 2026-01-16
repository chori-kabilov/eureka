using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using RazorWebApp.Services;

namespace RazorWebApp.Pages.Lessons;

[Authorize(Roles = "Admin")]
public class GradesModel : PageModel
{
    private readonly LessonsService _lessonsService;

    public GradesModel(LessonsService lessonsService)
    {
        _lessonsService = lessonsService;
    }

    public LessonDetailViewModel? Lesson { get; set; }
    public List<StudentGradeItem> Students { get; set; } = new();
    public string GradingSystemName { get; set; } = "5-балльная";
    public decimal MinScore { get; set; } = 1;
    public decimal MaxScore { get; set; } = 5;
    public decimal PassingScore { get; set; } = 3;
    public string? SuccessMessage { get; set; }

    public async Task<IActionResult> OnGetAsync(Guid lessonId, bool? success)
    {
        if (success == true)
            SuccessMessage = "Оценки сохранены";

        var response = await _lessonsService.GetLessonWithGradesAsync(lessonId);
        if (response == null)
            return RedirectToPage("/Groups/Index");

        Lesson = response.Lesson;
        Students = response.Students;
        GradingSystemName = response.GradingSystemName ?? "5-балльная";
        MinScore = response.MinScore;
        MaxScore = response.MaxScore;
        PassingScore = response.PassingScore;
        
        return Page();
    }

    public async Task<IActionResult> OnPostAsync(Guid lessonId, List<GradeItem> items)
    {
        var result = await _lessonsService.SaveGradesAsync(lessonId, items);
        
        return RedirectToPage(new { lessonId, success = result });
    }
}

// ViewModels
public class StudentGradeItem
{
    public Guid? StudentId { get; set; }
    public Guid? ChildId { get; set; }
    public string Name { get; set; } = string.Empty;
    public bool IsChild { get; set; }
    public decimal? Score { get; set; }
    public decimal Weight { get; set; } = 1;
    public string? Comment { get; set; }
}

public class GradeItem
{
    public Guid? StudentId { get; set; }
    public Guid? ChildId { get; set; }
    public decimal? Score { get; set; }
    public decimal Weight { get; set; } = 1;
    public string? Comment { get; set; }
}

public class LessonGradesResponse
{
    public LessonDetailViewModel Lesson { get; set; } = null!;
    public List<StudentGradeItem> Students { get; set; } = new();
    public string? GradingSystemName { get; set; }
    public decimal MinScore { get; set; }
    public decimal MaxScore { get; set; }
    public decimal PassingScore { get; set; }
}
