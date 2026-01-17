namespace Application.Modules.Teachers.UseCases.CreateTeacher;

// Request для создания учителя
public class CreateTeacherRequest
{
    public Guid UserId { get; set; }
    public List<string> Subjects { get; set; } = new();
    public int PaymentType { get; set; }
    public decimal? HourlyRate { get; set; }
    public string? Bio { get; set; }
}
