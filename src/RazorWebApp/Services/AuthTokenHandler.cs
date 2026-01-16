using System.Net.Http.Headers;

namespace RazorWebApp.Services;

// Handler который автоматически добавляет JWT токен из cookie в API запросы
public class AuthTokenHandler(IHttpContextAccessor httpContextAccessor) : DelegatingHandler
{
    protected override async Task<HttpResponseMessage> SendAsync(
        HttpRequestMessage request,
        CancellationToken cancellationToken)
    {
        // Получаем токен из claims пользователя
        var token = httpContextAccessor.HttpContext?.User
            .FindFirst("Token")?.Value;

        if (!string.IsNullOrEmpty(token))
        {
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);
        }

        return await base.SendAsync(request, cancellationToken);
    }
}
