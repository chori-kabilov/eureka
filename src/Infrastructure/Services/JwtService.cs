using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Application.Abstractions;
using Domain.Users;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace Infrastructure.Services;

// Генерация JWT токенов
public class JwtService(IConfiguration configuration) : IJwtService
{
    public string GenerateToken(User user)
    {
        var jwtSettings = configuration.GetSection("Jwt");
        var secret = jwtSettings["Secret"]!;
        var issuer = jwtSettings["Issuer"];
        var audience = jwtSettings["Audience"];
        var expirationHours = int.Parse(jwtSettings["ExpirationHours"] ?? "24");

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secret));
        var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var claims = new List<Claim>
        {
            new(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
            new(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            new(ClaimTypes.NameIdentifier, user.Id.ToString()),
            new(ClaimTypes.Name, user.FullName),
            new(ClaimTypes.MobilePhone, user.Phone)
        };

        // Добавляем роли на основе профилей
        if (user.AdminProfile != null)
            claims.Add(new Claim(ClaimTypes.Role, "Admin"));
        if (user.StudentProfile != null)
            claims.Add(new Claim(ClaimTypes.Role, "Student"));
        if (user.TeacherProfile != null)
            claims.Add(new Claim(ClaimTypes.Role, "Teacher"));
        if (user.ParentProfile != null)
            claims.Add(new Claim(ClaimTypes.Role, "Parent"));
        
        // Базовая роль если нет профилей
        if (user.AdminProfile == null && user.StudentProfile == null && 
            user.TeacherProfile == null && user.ParentProfile == null)
            claims.Add(new Claim(ClaimTypes.Role, "User"));

        var token = new JwtSecurityToken(
            issuer: issuer,
            audience: audience,
            claims: claims,
            expires: DateTime.UtcNow.AddHours(expirationHours),
            signingCredentials: credentials
        );

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}
