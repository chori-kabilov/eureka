using RazorWebApp.Models.Common;
using RazorWebApp.Models.Courses;

namespace RazorWebApp.Services;

// Сервис для работы с курсами
public class CoursesService
{
    private readonly ApiClient _apiClient;

    public CoursesService(ApiClient apiClient)
    {
        _apiClient = apiClient;
    }

    public async Task<PagedResponse<CourseViewModel>?> ListAsync(
        string? search = null, 
        bool? isArchived = null,
        int page = 1)
    {
        var url = $"/api/v1/courses?page={page}&pageSize=20";
        
        if (!string.IsNullOrEmpty(search))
            url += $"&search={Uri.EscapeDataString(search)}";
        if (isArchived.HasValue)
            url += $"&isArchived={isArchived.Value.ToString().ToLower()}";

        return await _apiClient.GetAsync<PagedResponse<CourseViewModel>>(url);
    }

    public async Task<ApiResponse<CourseViewModel>?> GetAsync(Guid id)
    {
        return await _apiClient.GetAsync<ApiResponse<CourseViewModel>>($"/api/v1/courses/{id}");
    }

    public async Task<ApiResponse<CourseViewModel>?> CreateAsync(
        string name, string? description, decimal price, int durationHours, int maxStudents)
    {
        var request = new 
        { 
            Name = name, 
            Description = description, 
            Price = price,
            DurationHours = durationHours,
            MaxStudents = maxStudents
        };
        return await _apiClient.PostAsync<object, ApiResponse<CourseViewModel>>("/api/v1/courses", request);
    }

    public async Task<ApiResponse<CourseViewModel>?> UpdateAsync(
        Guid id, string name, string? description, decimal price, int durationHours, int maxStudents)
    {
        var request = new 
        { 
            Name = name, 
            Description = description, 
            Price = price,
            DurationHours = durationHours,
            MaxStudents = maxStudents
        };
        return await _apiClient.PutAsync<object, ApiResponse<CourseViewModel>>($"/api/v1/courses/{id}", request);
    }

    public async Task<bool> ArchiveAsync(Guid id)
    {
        var result = await _apiClient.PostAsync<object, ApiResponse<object>>($"/api/v1/courses/{id}/archive", new {});
        return result?.Success == true;
    }

    public async Task<bool> DeleteAsync(Guid id)
    {
        return await _apiClient.DeleteAsync($"/api/v1/courses/{id}");
    }
}
