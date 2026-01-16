using RazorWebApp.Models.Auth;
using RazorWebApp.Models.Common;

namespace RazorWebApp.Services;

// Сервис аутентификации
public class AuthService(ApiClient api)
{
    public async Task<ApiResponse<AuthResult>?> RegisterAsync(RegisterRequest request)
    {
        return await api.PostAsync<RegisterRequest, ApiResponse<AuthResult>>(
            "/api/v1/auth/register", request);
    }

    public async Task<ApiResponse<AuthResult>?> LoginAsync(LoginRequest request)
    {
        return await api.PostAsync<LoginRequest, ApiResponse<AuthResult>>(
            "/api/v1/auth/login", request);
    }
}
