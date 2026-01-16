using Domain.Groups;

namespace Application.Modules.Groups.UseCases.ListGroups;

// Запрос списка групп
public class ListGroupsRequest
{
    public string? Search { get; set; }
    public Guid? CourseId { get; set; }
    public Guid? TeacherId { get; set; }
    public GroupStatus? Status { get; set; }
    public int Page { get; set; } = 1;
    public int PageSize { get; set; } = 20;
}
