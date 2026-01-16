using RazorWebApp.Models.Common;

namespace RazorWebApp.Services;

// Сервис для работы с кабинетами
public class RoomsService(ApiClient apiClient)
{
    public async Task<ApiResponse<List<RoomViewModel>>?> ListAsync()
    {
        return await apiClient.GetAsync<ApiResponse<List<RoomViewModel>>>("/api/v1/rooms");
    }

    public async Task<ApiResponse<RoomViewModel>?> CreateAsync(string name, string? code, int? capacity)
    {
        var request = new { Name = name, Code = code, Capacity = capacity };
        return await apiClient.PostAsync<object, ApiResponse<RoomViewModel>>("/api/v1/rooms", request);
    }

    public async Task<bool> UpdateAsync(Guid id, string name, string? code, int? capacity, bool isActive)
    {
        var request = new { Name = name, Code = code, Capacity = capacity, IsActive = isActive };
        var result = await apiClient.PutAsync<object, ApiResponse<object>>($"/api/v1/rooms/{id}", request);
        return result?.Success == true;
    }

    public async Task<bool> DeleteAsync(Guid id)
    {
        return await apiClient.DeleteAsync($"/api/v1/rooms/{id}");
    }
}

