using Application.Modules.Parents.Dtos;
using Domain.Parents;

namespace Application.Modules.Parents.Mapping;

// Маппинг Parent → DTO
public static class ParentMapper
{
    public static ParentDto ToDto(Parent parent) => new()
    {
        Id = parent.Id,
        UserId = parent.UserId,
        FullName = parent.User?.FullName ?? string.Empty,
        Phone = parent.User?.Phone ?? string.Empty,
        ChildrenCount = parent.Children?.Count ?? 0,
        CreatedAt = parent.CreatedAt
    };
}
