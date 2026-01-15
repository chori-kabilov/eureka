using Application.Modules.Courses.UseCases.ArchiveCourse;
using Application.Modules.Courses.UseCases.CreateCourse;
using Application.Modules.Courses.UseCases.DeleteCourse;
using Application.Modules.Courses.UseCases.GetCourse;
using Application.Modules.Courses.UseCases.ListCourses;
using Application.Modules.Courses.UseCases.UpdateCourse;
using Application.Modules.Auth.UseCases.Register;
using Application.Modules.Auth.UseCases.Login;
using Microsoft.Extensions.DependencyInjection;

namespace Application;

// Extension methods для регистрации сервисов Application
public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        // Auth handlers
        services.AddScoped<RegisterHandler>();
        services.AddScoped<LoginHandler>();

        // Courses handlers
        services.AddScoped<CreateCourseHandler>();
        services.AddScoped<UpdateCourseHandler>();
        services.AddScoped<GetCourseHandler>();
        services.AddScoped<ListCoursesHandler>();
        services.AddScoped<ArchiveCourseHandler>();
        services.AddScoped<DeleteCourseHandler>();

        return services;
    }
}
