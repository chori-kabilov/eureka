using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using RazorWebApp.Models.Auth;
using RazorWebApp.Services;
using System.Security.Claims;

namespace RazorWebApp.Pages.Auth;

public class LoginModel : PageModel
{
    private readonly AuthService _authService;
    private readonly ApiClient _apiClient;

    public LoginModel(AuthService authService, ApiClient apiClient)
    {
        _authService = authService;
        _apiClient = apiClient;
    }

    [BindProperty]
    public LoginRequest Input { get; set; } = new();

    public string? ErrorMessage { get; set; }

    public void OnGet()
    {
    }

    public async Task<IActionResult> OnPostAsync()
    {
        if (!ModelState.IsValid)
            return Page();

        var response = await _authService.LoginAsync(Input);

        if (response == null)
        {
            ErrorMessage = "Ошибка подключения к серверу";
            return Page();
        }

        if (!response.Success || response.Data == null)
        {
            ErrorMessage = response.Error?.Message ?? "Ошибка входа";
            return Page();
        }

        // Сохраняем токен в claims
        var claims = new List<Claim>
        {
            new(ClaimTypes.NameIdentifier, response.Data.UserId.ToString()),
            new(ClaimTypes.Name, response.Data.FullName),
            new(ClaimTypes.MobilePhone, response.Data.Phone),
            new(ClaimTypes.Role, response.Data.RoleName),
            new("Token", response.Data.Token)
        };

        var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
        var principal = new ClaimsPrincipal(identity);

        await HttpContext.SignInAsync(
            CookieAuthenticationDefaults.AuthenticationScheme,
            principal,
            new AuthenticationProperties { IsPersistent = true });

        return RedirectToPage("/Dashboard/Index");
    }
}
