using RazorWebApp.Models.Common;
using RazorWebApp.Models.Users;

namespace RazorWebApp.Services;

// Сервис для работы с пользователями
public class UsersService
{
    private readonly ApiClient _api;

    public UsersService(ApiClient api)
    {
        _api = api;
    }

    public async Task<UsersPagedResponse?> ListAsync(string? search = null, bool? isAdmin = null, int page = 1)
    {
        var url = $"/api/v1/users?page={page}";
        if (!string.IsNullOrEmpty(search))
            url += $"&search={Uri.EscapeDataString(search)}";
        if (isAdmin.HasValue)
            url += $"&isAdmin={isAdmin.Value}";
            
        return await _api.GetAsync<UsersPagedResponse>(url);
    }

    public async Task<ApiResponse<UserViewModel>?> GetAsync(Guid id)
    {
        return await _api.GetAsync<ApiResponse<UserViewModel>>($"/api/v1/users/{id}");
    }

    public async Task<ApiResponse<UserViewModel>?> UpdateAdminAsync(Guid id, bool isAdmin)
    {
        var request = new { IsAdmin = isAdmin };
        return await _api.PatchAsync<object, ApiResponse<UserViewModel>>($"/api/v1/users/{id}/admin", request);
    }

    public async Task<bool> DeleteAsync(Guid id)
    {
        return await _api.DeleteAsync($"/api/v1/users/{id}");
    }
}
