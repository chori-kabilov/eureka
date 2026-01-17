using Application.Modules.Teachers.Dtos;
using Domain.Teachers;

namespace Application.Modules.Teachers.Mapping;

// Маппинг Teacher -> DTO
public static class TeacherMapper
{
    public static TeacherDto ToDto(Teacher teacher) => new()
    {
        Id = teacher.Id,
        UserId = teacher.UserId,
        FullName = teacher.User?.FullName ?? string.Empty,
        Phone = teacher.User?.Phone ?? string.Empty,
        Status = teacher.Status,
        Subjects = teacher.Subjects,
        PaymentType = teacher.PaymentType,
        CreatedAt = teacher.CreatedAt
    };

    public static TeacherDetailDto ToDetailDto(Teacher teacher) => new()
    {
        Id = teacher.Id,
        UserId = teacher.UserId,
        FullName = teacher.User?.FullName ?? string.Empty,
        Phone = teacher.User?.Phone ?? string.Empty,
        Status = teacher.Status,
        Subjects = teacher.Subjects,
        PaymentType = teacher.PaymentType,
        HourlyRate = teacher.HourlyRate,
        Bio = teacher.Bio,
        HiredAt = teacher.HiredAt,
        CreatedAt = teacher.CreatedAt,
        UpdatedAt = teacher.UpdatedAt
    };
}
