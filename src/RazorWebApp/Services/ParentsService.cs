using RazorWebApp.Models.Common;
using RazorWebApp.Models.Parents;

namespace RazorWebApp.Services;

// Сервис для работы с родителями
public class ParentsService(ApiClient apiClient)
{
    public async Task<PagedResponse<ParentViewModel>?> ListAsync(string? search = null, int page = 1, int pageSize = 12)
    {
        var url = $"/api/v1/parents?page={page}&pageSize={pageSize}";
        
        if (!string.IsNullOrEmpty(search))
            url += $"&search={Uri.EscapeDataString(search)}";

        return await apiClient.GetAsync<PagedResponse<ParentViewModel>>(url);
    }

    public async Task<ParentViewModel?> GetAsync(Guid id)
    {
        return await apiClient.GetAsync<ParentViewModel>($"/api/v1/parents/{id}");
    }

    public async Task<ApiResponse<ParentViewModel>?> CreateAsync(Guid userId)
    {
        var request = new { UserId = userId };
        return await apiClient.PostAsync<object, ApiResponse<ParentViewModel>>("/api/v1/parents", request);
    }

    public async Task<bool> UpdateAsync(Guid id, UpdateParentRequest request)
    {
        var result = await apiClient.PatchAsync<UpdateParentRequest, ApiResponse<object>>($"/api/v1/parents/{id}", request);
        return result?.Success == true;
    }

    public async Task<bool> DeleteAsync(Guid id)
    {
        return await apiClient.DeleteAsync($"/api/v1/parents/{id}");
    }
}
