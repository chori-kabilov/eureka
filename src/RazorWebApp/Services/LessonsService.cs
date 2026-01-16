using RazorWebApp.Models.Common;
using RazorWebApp.Pages.Lessons;

namespace RazorWebApp.Services;

// Сервис для работы с занятиями
public class LessonsService(ApiClient apiClient)
{
    // Получить занятие с посещаемостью
    public async Task<LessonAttendanceResponse?> GetLessonWithAttendanceAsync(Guid lessonId)
    {
        return await apiClient.GetAsync<LessonAttendanceResponse>($"/api/v1/lessons/{lessonId}/attendance-form");
    }

    // Сохранить посещаемость
    public async Task<bool> SaveAttendanceAsync(Guid lessonId, List<AttendanceItem> items)
    {
        var request = new
        {
            LessonId = lessonId,
            Items = items.Select(i => new { i.StudentId, i.ChildId, i.Status })
        };
        var result = await apiClient.PostAsync<object, ApiResponse<object>>($"/api/v1/journal/lessons/{lessonId}/attendance/bulk", request);
        return result?.Success == true;
    }

    // Получить занятие с оценками
    public async Task<LessonGradesResponse?> GetLessonWithGradesAsync(Guid lessonId)
    {
        return await apiClient.GetAsync<LessonGradesResponse>($"/api/v1/lessons/{lessonId}/grades-form");
    }

    // Сохранить оценки
    public async Task<bool> SaveGradesAsync(Guid lessonId, List<GradeItem> items)
    {
        var saved = 0;
        foreach (var item in items.Where(i => i.Score.HasValue))
        {
            var request = new
            {
                LessonId = lessonId,
                StudentId = item.StudentId,
                ChildId = item.ChildId,
                Score = item.Score,
                Weight = item.Weight,
                Comment = item.Comment
            };
            var result = await apiClient.PostAsync<object, ApiResponse<object>>($"/api/v1/journal/lessons/{lessonId}/grades", request);
            if (result?.Success == true) saved++;
        }
        return saved > 0;
    }
}
