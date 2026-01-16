namespace Application.Modules.Groups.UseCases.CreateGroup;

// Запрос на создание группы
public class CreateGroupRequest
{
    public string Name { get; set; } = string.Empty;
    public string? Code { get; set; }
    public Guid CourseId { get; set; }
    public Guid ResponsibleTeacherId { get; set; }
    public Guid? DefaultTeacherId { get; set; }
    public Guid? DefaultRoomId { get; set; }
    public Guid? GradingSystemId { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime? EndDate { get; set; }
    public int MaxStudents { get; set; } = 15;
    public string? Notes { get; set; }
}
