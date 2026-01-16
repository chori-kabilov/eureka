using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using RazorWebApp.Models.Groups;
using RazorWebApp.Services;

namespace RazorWebApp.Pages.Groups;

[Authorize(Roles = "Admin")]
public class JournalModel : PageModel
{
    private readonly GroupsService _groupsService;
    private readonly JournalService _journalService;

    public JournalModel(GroupsService groupsService, JournalService journalService)
    {
        _groupsService = groupsService;
        _journalService = journalService;
    }

    public GroupViewModel? Group { get; set; }
    public List<LessonViewModel> Lessons { get; set; } = new();
    public List<JournalRowViewModel> Journal { get; set; } = new();

    public async Task<IActionResult> OnGetAsync(Guid id)
    {
        var response = await _groupsService.GetAsync(id);
        if (response?.Success != true || response.Data == null)
            return RedirectToPage("/Groups/Index");

        Group = response.Data;

        // Получаем журнал
        var journalResponse = await _journalService.GetGroupJournalAsync(id);
        if (journalResponse?.Success == true && journalResponse.Data != null)
        {
            Journal = journalResponse.Data;
            
            // Извлекаем уникальные занятия из первой строки
            if (Journal.Any())
            {
                Lessons = Journal.First().Cells.Select(c => new LessonViewModel
                {
                    Id = c.LessonId,
                    Date = c.Date,
                    Type = c.LessonType
                }).ToList();
            }
        }

        return Page();
    }
}

// ViewModels для журнала
public class LessonViewModel
{
    public Guid Id { get; set; }
    public DateTime Date { get; set; }
    public int Type { get; set; }
}

public class JournalRowViewModel
{
    public Guid? StudentId { get; set; }
    public Guid? ChildId { get; set; }
    public string StudentName { get; set; } = string.Empty;
    public bool IsChild { get; set; }
    public List<JournalCellViewModel> Cells { get; set; } = new();
    public decimal? AverageGrade { get; set; }
    public int AttendancePercent { get; set; }
}

public class JournalCellViewModel
{
    public Guid LessonId { get; set; }
    public DateTime Date { get; set; }
    public int LessonType { get; set; }
    public int? AttendanceStatus { get; set; }
    public decimal? Grade { get; set; }
}
