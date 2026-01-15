using RazorWebApp.Models.Common;
using RazorWebApp.Models.Parents;

namespace RazorWebApp.Services;

// Сервис для работы с родителями
public class ParentsService
{
    private readonly ApiClient _apiClient;

    public ParentsService(ApiClient apiClient)
    {
        _apiClient = apiClient;
    }

    public async Task<PagedResponse<ParentViewModel>?> ListAsync(string? search = null, int page = 1)
    {
        var url = $"/api/v1/parents?page={page}&pageSize=20";
        
        if (!string.IsNullOrEmpty(search))
            url += $"&search={Uri.EscapeDataString(search)}";

        return await _apiClient.GetAsync<PagedResponse<ParentViewModel>>(url);
    }

    public async Task<ApiResponse<ParentViewModel>?> CreateAsync(Guid userId)
    {
        var request = new { UserId = userId };
        return await _apiClient.PostAsync<object, ApiResponse<ParentViewModel>>("/api/v1/parents", request);
    }

    public async Task<bool> DeleteAsync(Guid id)
    {
        return await _apiClient.DeleteAsync($"/api/v1/parents/{id}");
    }
}
