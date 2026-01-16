using RazorWebApp.Models.Common;
using RazorWebApp.Pages.Groups;

namespace RazorWebApp.Services;

// Сервис для работы с расписанием
public class ScheduleService
{
    private readonly ApiClient _apiClient;

    public ScheduleService(ApiClient apiClient)
    {
        _apiClient = apiClient;
    }

    // Шаблоны расписания
    public async Task<ApiResponse<List<ScheduleTemplateViewModel>>?> GetTemplatesAsync(Guid groupId)
    {
        return await _apiClient.GetAsync<ApiResponse<List<ScheduleTemplateViewModel>>>($"/api/v1/schedule/groups/{groupId}/templates");
    }

    public async Task<bool> CreateTemplateAsync(Guid groupId, DayOfWeek dayOfWeek, TimeSpan startTime, TimeSpan endTime, Guid? roomId)
    {
        var request = new
        {
            GroupId = groupId,
            DayOfWeek = (int)dayOfWeek,
            StartTime = startTime.ToString(@"hh\:mm\:ss"),
            EndTime = endTime.ToString(@"hh\:mm\:ss"),
            RoomId = roomId
        };
        var result = await _apiClient.PostAsync<object, ApiResponse<object>>($"/api/v1/schedule/groups/{groupId}/templates", request);
        return result?.Success == true;
    }

    // Занятия
    public async Task<ApiResponse<List<LessonListViewModel>>?> GetLessonsAsync(Guid groupId)
    {
        return await _apiClient.GetAsync<ApiResponse<List<LessonListViewModel>>>($"/api/v1/lessons?groupId={groupId}&pageSize=50");
    }

    public async Task<int> GenerateLessonsAsync(Guid groupId, DateTime fromDate, DateTime toDate)
    {
        var request = new
        {
            GroupId = groupId,
            FromDate = fromDate.ToString("yyyy-MM-dd"),
            ToDate = toDate.ToString("yyyy-MM-dd")
        };
        var result = await _apiClient.PostAsync<object, GenerateResponse>("/api/v1/lessons/generate", request);
        return result?.Generated ?? 0;
    }

    private class GenerateResponse
    {
        public int Generated { get; set; }
    }
}
