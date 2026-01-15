using RazorWebApp.Models.Auth;
using RazorWebApp.Models.Common;

namespace RazorWebApp.Services;

// Сервис аутентификации
public class AuthService
{
    private readonly ApiClient _api;

    public AuthService(ApiClient api)
    {
        _api = api;
    }

    public async Task<ApiResponse<AuthResult>?> RegisterAsync(RegisterRequest request)
    {
        return await _api.PostAsync<RegisterRequest, ApiResponse<AuthResult>>(
            "/api/v1/auth/register", request);
    }

    public async Task<ApiResponse<AuthResult>?> LoginAsync(LoginRequest request)
    {
        return await _api.PostAsync<LoginRequest, ApiResponse<AuthResult>>(
            "/api/v1/auth/login", request);
    }
}
