using RazorWebApp.Models.Common;
using RazorWebApp.Models.Children;

namespace RazorWebApp.Services;

// Сервис для работы с детьми
public class ChildrenService(ApiClient apiClient)
{
    public async Task<PagedResponse<ChildViewModel>?> ListAsync(
        string? search = null, 
        Guid? parentId = null,
        int page = 1)
    {
        var url = $"/api/v1/children?page={page}&pageSize=20";
        
        if (!string.IsNullOrEmpty(search))
            url += $"&search={Uri.EscapeDataString(search)}";
        if (parentId.HasValue)
            url += $"&parentId={parentId}";

        return await apiClient.GetAsync<PagedResponse<ChildViewModel>>(url);
    }

    public async Task<ApiResponse<ChildViewModel>?> CreateAsync(
        Guid parentId, string fullName, DateTime? birthDate, string? notes)
    {
        var request = new 
        { 
            ParentId = parentId, 
            FullName = fullName, 
            BirthDate = birthDate,
            Notes = notes
        };
        return await apiClient.PostAsync<object, ApiResponse<ChildViewModel>>("/api/v1/children", request);
    }

    public async Task<ApiResponse<ChildViewModel>?> GetAsync(Guid id)
    {
        return await apiClient.GetAsync<ApiResponse<ChildViewModel>>($"/api/v1/children/{id}");
    }

    public async Task<ApiResponse<ChildViewModel>?> UpdateAsync(Guid id, int status, string? notes)
    {
        var request = new { Status = status, Notes = notes };
        return await apiClient.PatchAsync<object, ApiResponse<ChildViewModel>>($"/api/v1/children/{id}", request);
    }

    public async Task<bool> DeleteAsync(Guid id)
    {
        return await apiClient.DeleteAsync($"/api/v1/children/{id}");
    }
}
