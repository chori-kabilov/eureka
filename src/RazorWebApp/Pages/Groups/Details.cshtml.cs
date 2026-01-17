using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using RazorWebApp.Models.Groups;
using RazorWebApp.Services;

namespace RazorWebApp.Pages.Groups;

[Authorize(Roles = "Admin")]
public class DetailsModel : PageModel
{
    private readonly GroupsService _groupsService;
    private readonly StudentsService _studentsService;
    private readonly ChildrenService _childrenService;
    private readonly ScheduleService _scheduleService;
    private readonly RoomsService _roomsService;

    public DetailsModel(
        GroupsService groupsService,
        StudentsService studentsService,
        ChildrenService childrenService,
        ScheduleService scheduleService,
        RoomsService roomsService)
    {
        _groupsService = groupsService;
        _studentsService = studentsService;
        _childrenService = childrenService;
        _scheduleService = scheduleService;
        _roomsService = roomsService;
    }

    public GroupViewModel? Group { get; set; }
    public List<EnrollmentViewModel> Enrollments { get; set; } = new();
    public List<ScheduleTemplateViewModel> ScheduleTemplates { get; set; } = new();
    public List<LessonListViewModel> Lessons { get; set; } = new();
    public List<StudentSelectItem> AvailableStudents { get; set; } = new();
    public List<ChildSelectItem> AvailableChildren { get; set; } = new();
    public List<RoomSelectItem> Rooms { get; set; } = new();
    public string Tab { get; set; } = "students";
    public int? GeneratedCount { get; set; }

    public async Task<IActionResult> OnGetAsync(Guid id, string? tab, int? generated)
    {
        Tab = tab ?? "students";
        GeneratedCount = generated;
        
        var response = await _groupsService.GetAsync(id);
        if (response?.Success != true || response.Data == null)
            return RedirectToPage("/Groups/Index");

        Group = response.Data;
        
        // Загружаем студентов группы
        var studentsResponse = await _groupsService.GetStudentsAsync(id);
        Enrollments = studentsResponse?.Data ?? new List<EnrollmentViewModel>();

        if (Tab == "students")
        {
            await LoadStudentsData();
        }
        else if (Tab == "schedule")
        {
            await LoadScheduleData(id);
        }
        
        return Page();
    }

    private async Task LoadStudentsData()
    {
        // Загружаем доступных студентов
        var studentsResult = await _studentsService.ListAsync();
        var enrolledStudentIds = Enrollments.Where(e => e.StudentId.HasValue).Select(e => e.StudentId!.Value).ToHashSet();
        AvailableStudents = studentsResult?.Items?
            .Where(s => !enrolledStudentIds.Contains(s.Id))
            .Select(s => new StudentSelectItem { Id = s.Id, FullName = s.FullName, Phone = s.Phone })
            .ToList() ?? new();

        // Загружаем доступных детей
        var childrenResult = await _childrenService.ListAsync();
        var enrolledChildIds = Enrollments.Where(e => e.ChildId.HasValue).Select(e => e.ChildId!.Value).ToHashSet();
        AvailableChildren = childrenResult?.Items?
            .Where(c => !enrolledChildIds.Contains(c.Id))
            .Select(c => new ChildSelectItem { Id = c.Id, FullName = c.FullName, ParentName = c.ParentName })
            .ToList() ?? new();
    }

    private async Task LoadScheduleData(Guid groupId)
    {
        // Загружаем шаблоны
        var templatesResult = await _scheduleService.GetTemplatesAsync(groupId);
        ScheduleTemplates = templatesResult?.Data ?? new();

        // Загружаем занятия
        var lessonsResult = await _scheduleService.GetLessonsAsync(groupId);
        Lessons = lessonsResult?.Data ?? new();

        // Загружаем кабинеты
        var roomsResult = await _roomsService.ListAsync();
        Rooms = roomsResult?.Data?.Select(r => new RoomSelectItem { Id = r.Id, Name = r.Name }).ToList() ?? new();
    }

    public async Task<IActionResult> OnPostUnenrollAsync(Guid id, Guid enrollmentId)
    {
        await _groupsService.UnenrollAsync(id, enrollmentId);
        return RedirectToPage("/Groups/Details", new { id, tab = "students" });
    }

    public async Task<IActionResult> OnPostEnrollAsync(Guid id, Guid? studentId, Guid? childId)
    {
        if (studentId.HasValue || childId.HasValue)
        {
            await _groupsService.EnrollAsync(id, studentId, childId);
        }
        return RedirectToPage("/Groups/Details", new { id, tab = "students" });
    }

    public async Task<IActionResult> OnPostAddTemplateAsync(Guid id, int dayOfWeek, string startTime, string endTime, Guid? roomId)
    {
        await _scheduleService.CreateTemplateAsync(id, (DayOfWeek)dayOfWeek, 
            TimeSpan.Parse(startTime), TimeSpan.Parse(endTime), roomId);
        return RedirectToPage("/Groups/Details", new { id, tab = "schedule" });
    }

    public async Task<IActionResult> OnPostGenerateLessonsAsync(Guid id, DateTime fromDate, DateTime toDate)
    {
        var result = await _scheduleService.GenerateLessonsAsync(id, fromDate, toDate);
        return RedirectToPage("/Groups/Details", new { id, tab = "schedule", generated = result });
    }

    public async Task<IActionResult> OnPostDeleteTemplateAsync(Guid id, Guid templateId)
    {
        await _scheduleService.DeleteTemplateAsync(templateId);
        return RedirectToPage("/Groups/Details", new { id, tab = "schedule" });
    }

    public async Task<IActionResult> OnPostCancelLessonAsync(Guid id, Guid lessonId, string? reason)
    {
        await _scheduleService.CancelLessonAsync(lessonId, reason);
        return RedirectToPage("/Groups/Details", new { id, tab = "schedule" });
    }
}

// Select Items
public class StudentSelectItem
{
    public Guid Id { get; set; }
    public string FullName { get; set; } = string.Empty;
    public string? Phone { get; set; }
}

public class ChildSelectItem
{
    public Guid Id { get; set; }
    public string FullName { get; set; } = string.Empty;
    public string? ParentName { get; set; }
}

public class RoomSelectItem
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
}

// ScheduleTemplate ViewModel
public class ScheduleTemplateViewModel
{
    public Guid Id { get; set; }
    public DayOfWeek DayOfWeek { get; set; }
    public TimeSpan StartTime { get; set; }
    public TimeSpan EndTime { get; set; }
    public Guid? RoomId { get; set; }
    public string? RoomName { get; set; }
    public bool IsActive { get; set; }

    public string DayName => DayOfWeek switch
    {
        DayOfWeek.Monday => "Понедельник",
        DayOfWeek.Tuesday => "Вторник",
        DayOfWeek.Wednesday => "Среда",
        DayOfWeek.Thursday => "Четверг",
        DayOfWeek.Friday => "Пятница",
        DayOfWeek.Saturday => "Суббота",
        DayOfWeek.Sunday => "Воскресенье",
        _ => "Неизвестно"
    };
}

// Lesson ViewModel для списка
public class LessonListViewModel
{
    public Guid Id { get; set; }
    public DateTime Date { get; set; }
    public TimeSpan StartTime { get; set; }
    public TimeSpan EndTime { get; set; }
    public int Type { get; set; }
    public int Status { get; set; }
    public string TeacherName { get; set; } = string.Empty;
    public string? RoomName { get; set; }

    public string DayName => Date.DayOfWeek switch
    {
        DayOfWeek.Monday => "Пн",
        DayOfWeek.Tuesday => "Вт",
        DayOfWeek.Wednesday => "Ср",
        DayOfWeek.Thursday => "Чт",
        DayOfWeek.Friday => "Пт",
        DayOfWeek.Saturday => "Сб",
        DayOfWeek.Sunday => "Вс",
        _ => ""
    };

    public string TypeName => Type switch
    {
        0 => "Урок",
        1 => "Экзамен",
        2 => "Консультация",
        3 => "Отработка",
        4 => "Замена",
        _ => "Неизвестно"
    };

    public string StatusName => Status switch
    {
        0 => "Запланировано",
        1 => "Идёт",
        2 => "Завершено",
        3 => "Отменено",
        _ => "Неизвестно"
    };

    public string StatusBadgeClass => Status switch
    {
        0 => "bg-secondary",
        1 => "bg-warning text-dark",
        2 => "bg-success",
        3 => "bg-danger",
        _ => "bg-secondary"
    };
}
