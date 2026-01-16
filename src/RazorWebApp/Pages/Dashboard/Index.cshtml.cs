using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.RazorPages;
using RazorWebApp.Services;

namespace RazorWebApp.Pages.Dashboard;

[Authorize]
public class IndexModel : PageModel
{
    private readonly StudentsService _studentsService;
    private readonly TeachersService _teachersService;
    private readonly CoursesService _coursesService;
    private readonly GroupsService _groupsService;

    public IndexModel(
        StudentsService studentsService,
        TeachersService teachersService,
        CoursesService coursesService,
        GroupsService groupsService)
    {
        _studentsService = studentsService;
        _teachersService = teachersService;
        _coursesService = coursesService;
        _groupsService = groupsService;
    }

    public int StudentsCount { get; set; }
    public int TeachersCount { get; set; }
    public int CoursesCount { get; set; }
    public int GroupsCount { get; set; }
    public List<RecentEnrollmentItem> RecentEnrollments { get; set; } = new();
    public List<PopularCourseItem> PopularCourses { get; set; } = new();

    public async Task OnGetAsync()
    {
        // Получаем статистику
        var students = await _studentsService.ListAsync();
        StudentsCount = students?.TotalCount ?? 0;

        var teachers = await _teachersService.ListAsync();
        TeachersCount = teachers?.TotalCount ?? 0;

        var courses = await _coursesService.ListAsync();
        CoursesCount = courses?.Items?.Count() ?? 0;

        var groups = await _groupsService.ListAsync();
        GroupsCount = groups?.Items?.Count() ?? 0;

        // Заглушки для данных активности
        RecentEnrollments = new List<RecentEnrollmentItem>
        {
            new() { StudentName = "Иван Петров", CourseName = "JavaScript для начинающих", TimeAgo = "2 часа назад" },
            new() { StudentName = "Мария Сидорова", CourseName = "Python разработка", TimeAgo = "4 часа назад" },
            new() { StudentName = "Создан новый курс", CourseName = "React.js Advanced", TimeAgo = "6 часов назад" }
        };

        PopularCourses = new List<PopularCourseItem>
        {
            new() { Name = "JavaScript для начинающих", StudentsCount = 45 },
            new() { Name = "Python разработка", StudentsCount = 38 },
            new() { Name = "React.js Advanced", StudentsCount = 29 }
        };
    }
}

public class RecentEnrollmentItem
{
    public string StudentName { get; set; } = string.Empty;
    public string CourseName { get; set; } = string.Empty;
    public string TimeAgo { get; set; } = string.Empty;
}

public class PopularCourseItem
{
    public string Name { get; set; } = string.Empty;
    public int StudentsCount { get; set; }
}
