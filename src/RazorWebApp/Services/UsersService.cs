using RazorWebApp.Models.Common;
using RazorWebApp.Models.Users;

namespace RazorWebApp.Services;

// Сервис для работы с пользователями
public class UsersService(ApiClient api)
{
    public async Task<UsersPagedResponse?> ListAsync(string? search = null, bool? isAdmin = null, int page = 1, int pageSize = 12)
    {
        var url = $"/api/v1/users?page={page}&pageSize={pageSize}";
        if (!string.IsNullOrEmpty(search))
            url += $"&search={Uri.EscapeDataString(search)}";
        if (isAdmin.HasValue)
            url += $"&isAdmin={isAdmin.Value}";
            
        return await api.GetAsync<UsersPagedResponse>(url);
    }

    public async Task<ApiResponse<UserViewModel>?> GetAsync(Guid id)
    {
        return await api.GetAsync<ApiResponse<UserViewModel>>($"/api/v1/users/{id}");
    }

    public async Task<ApiResponse<UserViewModel>?> UpdateAdminAsync(Guid id, bool isAdmin)
    {
        var request = new { IsAdmin = isAdmin };
        return await api.PatchAsync<object, ApiResponse<UserViewModel>>($"/api/v1/users/{id}/admin", request);
    }

    public async Task<bool> DeleteAsync(Guid id)
    {
        return await api.DeleteAsync($"/api/v1/users/{id}");
    }
}
