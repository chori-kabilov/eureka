using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.RazorPages;
using RazorWebApp.Services;

namespace RazorWebApp.Pages.Dashboard;

[Authorize]
public class IndexModel : PageModel
{
    private readonly DashboardService _dashboardService;

    public IndexModel(DashboardService dashboardService)
    {
        _dashboardService = dashboardService;
    }

    public int StudentsCount { get; set; }
    public int TeachersCount { get; set; }
    public int CoursesCount { get; set; }
    public int GroupsCount { get; set; }
    public List<RecentEnrollmentItem> RecentEnrollments { get; set; } = new();
    public List<PopularCourseItem> PopularCourses { get; set; } = new();

    public async Task OnGetAsync()
    {
        // Получаем реальную статистику
        var stats = await _dashboardService.GetStatsAsync();
        if (stats != null)
        {
            StudentsCount = stats.StudentsCount;
            TeachersCount = stats.TeachersCount;
            CoursesCount = stats.CoursesCount;
            GroupsCount = stats.GroupsCount;
        }

        // Получаем последние записи
        RecentEnrollments = await _dashboardService.GetRecentEnrollmentsAsync();

        // Получаем популярные курсы
        PopularCourses = await _dashboardService.GetPopularCoursesAsync();
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
