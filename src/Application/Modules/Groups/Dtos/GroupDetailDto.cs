namespace Application.Modules.Groups.Dtos;

// Детальный DTO группы
public class GroupDetailDto : GroupDto
{
    public Guid? DefaultTeacherId { get; set; }
    public string? DefaultTeacherName { get; set; }
    
    public Guid? DefaultRoomId { get; set; }
    public string? DefaultRoomName { get; set; }
    
    public Guid? GradingSystemId { get; set; }
    public string? GradingSystemName { get; set; }
    
    public string? Notes { get; set; }
    public DateTime? UpdatedAt { get; set; }
}
