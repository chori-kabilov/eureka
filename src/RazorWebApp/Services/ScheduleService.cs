using RazorWebApp.Models.Common;
using RazorWebApp.Pages.Groups;

namespace RazorWebApp.Services;

// Сервис для работы с расписанием
public class ScheduleService(ApiClient apiClient)
{
    // Шаблоны расписания
    public async Task<ApiResponse<List<ScheduleTemplateViewModel>>?> GetTemplatesAsync(Guid groupId)
    {
        return await apiClient.GetAsync<ApiResponse<List<ScheduleTemplateViewModel>>>($"/api/v1/schedule/groups/{groupId}/templates");
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
        var result = await apiClient.PostAsync<object, ApiResponse<object>>($"/api/v1/schedule/groups/{groupId}/templates", request);
        return result?.Success == true;
    }

    // Занятия
    public async Task<ApiResponse<List<LessonListViewModel>>?> GetLessonsAsync(Guid groupId)
    {
        return await apiClient.GetAsync<ApiResponse<List<LessonListViewModel>>>($"/api/v1/lessons?groupId={groupId}&pageSize=50");
    }

    public async Task<int> GenerateLessonsAsync(Guid groupId, DateTime fromDate, DateTime toDate)
    {
        var request = new
        {
            GroupId = groupId,
            FromDate = fromDate,
            ToDate = toDate
        };
        var result = await apiClient.PostAsync<object, GenerateResponse>("/api/v1/lessons/generate", request);
        return result?.Generated ?? 0;
    }

    public async Task<bool> DeleteTemplateAsync(Guid templateId)
    {
        return await apiClient.DeleteAsync($"/api/v1/schedule/templates/{templateId}");
    }

    public async Task<bool> CancelLessonAsync(Guid lessonId, string? reason)
    {
        var request = new { Reason = reason };
        var result = await apiClient.PostAsync<object, ApiResponse<object>>($"/api/v1/lessons/{lessonId}/cancel", request);
        return result?.Success == true;
    }

    private class GenerateResponse
    {
        public int Generated { get; set; }
    }
}
