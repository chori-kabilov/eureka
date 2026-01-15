using Application.Modules.Auth.Dtos;
using Application.Modules.Auth.UseCases.Login;
using Application.Modules.Auth.UseCases.Register;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebApi.Contracts.Auth;
using WebApi.Contracts.Common;
using WebApi.Extensions;

namespace WebApi.Controllers.v1;

[ApiController]
[Route("api/v1/[controller]")]
public class AuthController(RegisterHandler registerHandler, LoginHandler loginHandler) : ControllerBase
{
    // POST /api/v1/auth/register
    [HttpPost("register")]
    public async Task<IActionResult> Register(
        [FromBody] RegisterApiRequest apiRequest,
        CancellationToken ct)
    {
        var request = new RegisterRequest
        {
            Phone = apiRequest.Phone,
            FullName = apiRequest.FullName,
            Password = apiRequest.Password
        };

        var result = await registerHandler.HandleAsync(request, ct);

        if (result.IsSuccess)
            return CreatedAtAction(nameof(Me), ApiResponse<AuthResultDto>.Ok(result.Value!));

        return result.ToActionResult();
    }

    // POST /api/v1/auth/login
    [HttpPost("login")]
    public async Task<IActionResult> Login(
        [FromBody] LoginApiRequest apiRequest,
        CancellationToken ct)
    {
        var request = new LoginRequest
        {
            Phone = apiRequest.Phone,
            Password = apiRequest.Password
        };

        var result = await loginHandler.HandleAsync(request, ct);
        return result.ToActionResult();
    }

    // GET /api/v1/auth/me
    [Authorize]
    [HttpGet("me")]
    public IActionResult Me()
    {
        var userId = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
        var fullName = User.FindFirst(System.Security.Claims.ClaimTypes.Name)?.Value;
        var phone = User.FindFirst(System.Security.Claims.ClaimTypes.MobilePhone)?.Value;
        var role = User.FindFirst(System.Security.Claims.ClaimTypes.Role)?.Value;

        return Ok(ApiResponse<object>.Ok(new
        {
            userId,
            fullName,
            phone,
            role
        }));
    }
}
