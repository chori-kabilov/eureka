using RazorWebApp.Pages.Dashboard;

namespace RazorWebApp.Services;

// Сервис для получения данных Dashboard
public class DashboardService(ApiClient apiClient)
{
    public async Task<DashboardStats?> GetStatsAsync()
    {
        return await apiClient.GetAsync<DashboardStats>("/api/v1/dashboard/stats");
    }

    public async Task<List<RecentEnrollmentItem>> GetRecentEnrollmentsAsync()
    {
        var result = await apiClient.GetAsync<List<RecentEnrollmentItem>>("/api/v1/dashboard/recent-enrollments");
        return result ?? new List<RecentEnrollmentItem>();
    }

    public async Task<List<PopularCourseItem>> GetPopularCoursesAsync()
    {
        var result = await apiClient.GetAsync<List<PopularCourseItem>>("/api/v1/dashboard/popular-courses");
        return result ?? new List<PopularCourseItem>();
    }
}
