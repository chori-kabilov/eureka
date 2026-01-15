using Application.Modules.Users.Dtos;
using Domain.Users;

namespace Application.Modules.Users.Mapping;

// Маппинг User → DTO
public static class UserMapper
{
    public static UserDto ToDto(User user) => new()
    {
        Id = user.Id,
        Phone = user.Phone,
        FullName = user.FullName,
        IsAdmin = user.AdminProfile != null,
        IsStudent = user.StudentProfile != null,
        IsTeacher = user.TeacherProfile != null,
        IsParent = user.ParentProfile != null,
        CreatedAt = user.CreatedAt
    };

    public static UserDetailDto ToDetailDto(User user) => new()
    {
        Id = user.Id,
        Phone = user.Phone,
        FullName = user.FullName,
        IsAdmin = user.AdminProfile != null,
        IsStudent = user.StudentProfile != null,
        IsTeacher = user.TeacherProfile != null,
        IsParent = user.ParentProfile != null,
        CreatedAt = user.CreatedAt,
        UpdatedAt = user.UpdatedAt,
        IsDeleted = user.IsDeleted
    };
}
