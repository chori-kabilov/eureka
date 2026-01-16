using RazorWebApp.Models.Common;

namespace RazorWebApp.Services;

// Сервис для работы с кабинетами
public class RoomsService
{
    private readonly ApiClient _apiClient;

    public RoomsService(ApiClient apiClient)
    {
        _apiClient = apiClient;
    }

    public async Task<ApiResponse<List<RoomViewModel>>?> ListAsync()
    {
        return await _apiClient.GetAsync<ApiResponse<List<RoomViewModel>>>("/api/v1/rooms");
    }

    public async Task<ApiResponse<RoomViewModel>?> CreateAsync(string name, string? code, int? capacity)
    {
        var request = new { Name = name, Code = code, Capacity = capacity };
        return await _apiClient.PostAsync<object, ApiResponse<RoomViewModel>>("/api/v1/rooms", request);
    }
}

// ViewModel для кабинета
public class RoomViewModel
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string? Code { get; set; }
    public int? Capacity { get; set; }
    public bool IsActive { get; set; }
}
