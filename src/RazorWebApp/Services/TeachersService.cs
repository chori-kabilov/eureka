using RazorWebApp.Models.Common;
using RazorWebApp.Models.Teachers;

namespace RazorWebApp.Services;

// Сервис для работы с учителями
public class TeachersService
{
    private readonly ApiClient _apiClient;

    public TeachersService(ApiClient apiClient)
    {
        _apiClient = apiClient;
    }

    public async Task<PagedResponse<TeacherViewModel>?> ListAsync(string? search = null, int page = 1)
    {
        var url = $"/api/v1/teachers?page={page}&pageSize=20";
        
        if (!string.IsNullOrEmpty(search))
            url += $"&search={Uri.EscapeDataString(search)}";

        return await _apiClient.GetAsync<PagedResponse<TeacherViewModel>>(url);
    }

    public async Task<ApiResponse<TeacherViewModel>?> GetAsync(Guid id)
    {
        return await _apiClient.GetAsync<ApiResponse<TeacherViewModel>>($"/api/v1/teachers/{id}");
    }

    public async Task<ApiResponse<TeacherViewModel>?> CreateAsync(
        Guid userId, string? specialization, int paymentType, decimal? hourlyRate, string? bio)
    {
        var request = new 
        { 
            UserId = userId, 
            Specialization = specialization, 
            PaymentType = paymentType,
            HourlyRate = hourlyRate,
            Bio = bio
        };
        return await _apiClient.PostAsync<object, ApiResponse<TeacherViewModel>>("/api/v1/teachers", request);
    }

    public async Task<ApiResponse<TeacherViewModel>?> UpdateAsync(
        Guid id, string? specialization, int paymentType, decimal? hourlyRate, string? bio)
    {
        var request = new 
        { 
            Specialization = specialization, 
            PaymentType = paymentType,
            HourlyRate = hourlyRate,
            Bio = bio
        };
        return await _apiClient.PatchAsync<object, ApiResponse<TeacherViewModel>>($"/api/v1/teachers/{id}", request);
    }

    public async Task<bool> DeleteAsync(Guid id)
    {
        return await _apiClient.DeleteAsync($"/api/v1/teachers/{id}");
    }
}
