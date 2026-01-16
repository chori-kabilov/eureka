namespace Application.Modules.Students.Dtos;

// Детальный DTO студента
public class StudentDetailDto : StudentDto
{
    public string? Notes { get; set; }
    public DateTime? UpdatedAt { get; set; }
}
