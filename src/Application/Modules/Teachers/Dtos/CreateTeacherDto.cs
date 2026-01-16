namespace Application.Modules.Teachers.Dtos;

// DTO для создания учителя
public class CreateTeacherDto
{
    public Guid UserId { get; set; }
    public string? Specialization { get; set; }
    public int PaymentType { get; set; }
    public decimal? HourlyRate { get; set; }
    public string? Bio { get; set; }
}
