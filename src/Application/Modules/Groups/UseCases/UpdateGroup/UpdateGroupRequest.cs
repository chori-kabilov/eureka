using Domain.Groups;

namespace Application.Modules.Groups.UseCases.UpdateGroup;

// Запрос на обновление группы
public class UpdateGroupRequest
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string? Code { get; set; }
    public Guid ResponsibleTeacherId { get; set; }
    public Guid? DefaultTeacherId { get; set; }
    public Guid? DefaultRoomId { get; set; }
    public Guid? GradingSystemId { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime? EndDate { get; set; }
    public int MaxStudents { get; set; }
    public GroupStatus Status { get; set; }
    public string? Notes { get; set; }
}
