namespace Application.Modules.Children.Dtos;

// Детальный DTO ребёнка
public class ChildDetailDto : ChildDto
{
    public string? Notes { get; set; }
    public Guid? LinkedStudentId { get; set; }
    public DateTime? UpdatedAt { get; set; }
}
