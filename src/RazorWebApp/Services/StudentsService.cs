using RazorWebApp.Models.Common;
using RazorWebApp.Models.Students;

namespace RazorWebApp.Services;

// Сервис для работы со студентами
public class StudentsService(ApiClient apiClient)
{
    public async Task<PagedResponse<StudentViewModel>?> ListAsync(
        string? search = null, 
        int? status = null, 
        int page = 1,
        int pageSize = 12)
    {
        var url = $"/api/v1/students?page={page}&pageSize={pageSize}";
        
        if (!string.IsNullOrEmpty(search))
            url += $"&search={Uri.EscapeDataString(search)}";
        if (status.HasValue)
            url += $"&status={status}";

        return await apiClient.GetAsync<PagedResponse<StudentViewModel>>(url);
    }

    public async Task<ApiResponse<StudentViewModel>?> GetAsync(Guid id)
    {
        return await apiClient.GetAsync<ApiResponse<StudentViewModel>>($"/api/v1/students/{id}");
    }

    public async Task<ApiResponse<StudentViewModel>?> CreateAsync(Guid userId, string? notes)
    {
        var request = new { UserId = userId, Notes = notes };
        return await apiClient.PostAsync<object, ApiResponse<StudentViewModel>>("/api/v1/students", request);
    }

    public async Task<ApiResponse<StudentViewModel>?> UpdateAsync(
        Guid id, 
        string fullName, 
        string phone, 
        int status, 
        string? notes)
    {
        var request = new { FullName = fullName, Phone = phone, Status = status, Notes = notes };
        return await apiClient.PatchAsync<object, ApiResponse<StudentViewModel>>($"/api/v1/students/{id}", request);
    }

    public async Task<bool> DeleteAsync(Guid id)
    {
        return await apiClient.DeleteAsync($"/api/v1/students/{id}");
    }
}
