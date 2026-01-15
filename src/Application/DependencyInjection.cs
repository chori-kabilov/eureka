using Application.Modules.Courses.UseCases.ArchiveCourse;
using Application.Modules.Courses.UseCases.CreateCourse;
using Application.Modules.Courses.UseCases.DeleteCourse;
using Application.Modules.Courses.UseCases.GetCourse;
using Application.Modules.Courses.UseCases.ListCourses;
using Application.Modules.Courses.UseCases.UpdateCourse;
using Application.Modules.Auth.UseCases.Register;
using Application.Modules.Auth.UseCases.Login;
using Application.Modules.Users.UseCases.ListUsers;
using Application.Modules.Users.UseCases.GetUser;
using Application.Modules.Users.UseCases.UpdateUserRole;
using Application.Modules.Users.UseCases.DeleteUser;
using Application.Modules.Students.UseCases.ListStudents;
using Application.Modules.Students.UseCases.GetStudent;
using Application.Modules.Students.UseCases.CreateStudent;
using Application.Modules.Students.UseCases.UpdateStudent;
using Application.Modules.Students.UseCases.DeleteStudent;
using Application.Modules.Teachers.UseCases.ListTeachers;
using Application.Modules.Teachers.UseCases.GetTeacher;
using Application.Modules.Teachers.UseCases.CreateTeacher;
using Application.Modules.Teachers.UseCases.UpdateTeacher;
using Application.Modules.Teachers.UseCases.DeleteTeacher;
using Application.Modules.Children.UseCases.ListChildren;
using Application.Modules.Children.UseCases.CreateChild;
using Application.Modules.Children.UseCases.DeleteChild;
using Application.Modules.Parents.UseCases.ListParents;
using Application.Modules.Parents.UseCases.CreateParent;
using Application.Modules.Parents.UseCases.DeleteParent;
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

        // Users handlers
        services.AddScoped<ListUsersHandler>();
        services.AddScoped<GetUserHandler>();
        services.AddScoped<UpdateUserRoleHandler>();
        services.AddScoped<DeleteUserHandler>();

        // Students handlers
        services.AddScoped<ListStudentsHandler>();
        services.AddScoped<GetStudentHandler>();
        services.AddScoped<CreateStudentHandler>();
        services.AddScoped<UpdateStudentHandler>();
        services.AddScoped<DeleteStudentHandler>();

        // Teachers handlers
        services.AddScoped<ListTeachersHandler>();
        services.AddScoped<GetTeacherHandler>();
        services.AddScoped<CreateTeacherHandler>();
        services.AddScoped<UpdateTeacherHandler>();
        services.AddScoped<DeleteTeacherHandler>();

        // Children handlers
        services.AddScoped<ListChildrenHandler>();
        services.AddScoped<CreateChildHandler>();
        services.AddScoped<DeleteChildHandler>();

        // Parents handlers
        services.AddScoped<ListParentsHandler>();
        services.AddScoped<CreateParentHandler>();
        services.AddScoped<DeleteParentHandler>();

        return services;
    }
}
