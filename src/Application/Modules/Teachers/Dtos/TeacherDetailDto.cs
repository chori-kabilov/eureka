namespace Application.Modules.Teachers.Dtos;

// Детальный DTO учителя
public class TeacherDetailDto : TeacherDto
{
    public decimal? HourlyRate { get; set; }
    public string? Bio { get; set; }
    public DateTime? HiredAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
}
