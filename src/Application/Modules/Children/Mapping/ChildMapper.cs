using Application.Modules.Children.Dtos;
using Domain.Users;

namespace Application.Modules.Children.Mapping;

// Маппинг Child → DTO
public static class ChildMapper
{
    public static ChildDto ToDto(Child child) => new()
    {
        Id = child.Id,
        ParentId = child.ParentId,
        ParentName = child.Parent?.User?.FullName ?? string.Empty,
        FullName = child.FullName,
        BirthDate = child.BirthDate,
        Status = child.Status,
        CreatedAt = child.CreatedAt
    };

    public static ChildDetailDto ToDetailDto(Child child) => new()
    {
        Id = child.Id,
        ParentId = child.ParentId,
        ParentName = child.Parent?.User?.FullName ?? string.Empty,
        FullName = child.FullName,
        BirthDate = child.BirthDate,
        Status = child.Status,
        Notes = child.Notes,
        LinkedStudentId = child.LinkedStudentId,
        CreatedAt = child.CreatedAt,
        UpdatedAt = child.UpdatedAt
    };
}
