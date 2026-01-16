namespace Application.Modules.Teachers.Dtos;

// Детальный DTO учителя
public class TeacherDetailDto : TeacherDto
{
    public string? Bio { get; set; }
    public DateTime? UpdatedAt { get; set; }
}
