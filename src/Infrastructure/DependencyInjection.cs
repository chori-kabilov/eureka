using Application.Abstractions;
using Infrastructure.Persistence;
using Infrastructure.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure;

// Extension methods для регистрации сервисов Infrastructure
public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        // PostgreSQL
        var connectionString = configuration.GetConnectionString("Database");
        services.AddDbContext<DataContext>(options =>
            options.UseNpgsql(connectionString));

        // IDataContext
        services.AddScoped<IDataContext>(sp => sp.GetRequiredService<DataContext>());

        // Сервисы аутентификации
        services.AddScoped<ICurrentUser, CurrentUserService>();
        services.AddScoped<IPasswordHasher, PasswordHasher>();
        services.AddScoped<IJwtService, JwtService>();

        return services;
    }
}
