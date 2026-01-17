using Application.Modules.Students.Dtos;
using Domain.Students;

namespace Application.Modules.Students.Mapping;

// Маппинг Student → DTO
public static class StudentMapper
{
    public static StudentDto ToDto(Student student) => new()
    {
        Id = student.Id,
        UserId = student.UserId,
        FullName = student.User?.FullName ?? string.Empty,
        Phone = student.User?.Phone ?? string.Empty,
        Status = student.Status,
        CreatedAt = student.CreatedAt
    };

    public static StudentDetailDto ToDetailDto(Student student) => new()
    {
        Id = student.Id,
        UserId = student.UserId,
        FullName = student.User?.FullName ?? string.Empty,
        Phone = student.User?.Phone ?? string.Empty,
        Status = student.Status,
        Notes = student.Notes,
        CreatedAt = student.CreatedAt,
        UpdatedAt = student.UpdatedAt
    };
}
