using RazorWebApp.Models.Common;
using RazorWebApp.Models.Teachers;

namespace RazorWebApp.Services;

// Сервис для работы с учителями
public class TeachersService(ApiClient apiClient)
{
    public async Task<PagedResponse<TeacherViewModel>?> ListAsync(string? search = null, int page = 1, int pageSize = 12)
    {
        var url = $"/api/v1/teachers?page={page}&pageSize={pageSize}";
        
        if (!string.IsNullOrEmpty(search))
            url += $"&search={Uri.EscapeDataString(search)}";

        return await apiClient.GetAsync<PagedResponse<TeacherViewModel>>(url);
    }

    public async Task<ApiResponse<TeacherViewModel>?> GetAsync(Guid id)
    {
        return await apiClient.GetAsync<ApiResponse<TeacherViewModel>>($"/api/v1/teachers/{id}");
    }

    public async Task<ApiResponse<TeacherViewModel>?> CreateAsync(
        Guid userId, List<string> subjects, int paymentType, decimal? hourlyRate, string? bio)
    {
        var request = new 
        { 
            UserId = userId, 
            Subjects = subjects, 
            PaymentType = paymentType,
            HourlyRate = hourlyRate,
            Bio = bio
        };
        return await apiClient.PostAsync<object, ApiResponse<TeacherViewModel>>("/api/v1/teachers", request);
    }

    public async Task<ApiResponse<TeacherViewModel>?> UpdateAsync(
        Guid id, string fullName, string phone, int status, List<string> subjects, int paymentType, decimal? hourlyRate, string? bio)
    {
        var request = new 
        { 
            FullName = fullName,
            Phone = phone,
            Status = status,
            Subjects = subjects, 
            PaymentType = paymentType,
            HourlyRate = hourlyRate,
            Bio = bio
        };
        return await apiClient.PatchAsync<object, ApiResponse<TeacherViewModel>>($"/api/v1/teachers/{id}", request);
    }

    public async Task<bool> DeleteAsync(Guid id)
    {
        return await apiClient.DeleteAsync($"/api/v1/teachers/{id}");
    }
}

