using RazorWebApp.Models.Common;
using RazorWebApp.Models.Students;

namespace RazorWebApp.Services;

// Сервис для работы со студентами
public class StudentsService
{
    private readonly ApiClient _apiClient;

    public StudentsService(ApiClient apiClient)
    {
        _apiClient = apiClient;
    }

    public async Task<PagedResponse<StudentViewModel>?> ListAsync(
        string? search = null, 
        int? status = null, 
        int page = 1)
    {
        var url = $"/api/v1/students?page={page}&pageSize=20";
        
        if (!string.IsNullOrEmpty(search))
            url += $"&search={Uri.EscapeDataString(search)}";
        if (status.HasValue)
            url += $"&status={status}";

        return await _apiClient.GetAsync<PagedResponse<StudentViewModel>>(url);
    }

    public async Task<ApiResponse<StudentViewModel>?> GetAsync(Guid id)
    {
        return await _apiClient.GetAsync<ApiResponse<StudentViewModel>>($"/api/v1/students/{id}");
    }

    public async Task<ApiResponse<StudentViewModel>?> CreateAsync(Guid userId, string? notes)
    {
        var request = new { UserId = userId, Notes = notes };
        return await _apiClient.PostAsync<object, ApiResponse<StudentViewModel>>("/api/v1/students", request);
    }

    public async Task<ApiResponse<StudentViewModel>?> UpdateAsync(Guid id, int status, string? notes)
    {
        var request = new { Status = status, Notes = notes };
        return await _apiClient.PatchAsync<object, ApiResponse<StudentViewModel>>($"/api/v1/students/{id}", request);
    }

    public async Task<bool> DeleteAsync(Guid id)
    {
        return await _apiClient.DeleteAsync($"/api/v1/students/{id}");
    }
}
