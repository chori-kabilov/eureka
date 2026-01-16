namespace Application.Modules.Teachers.UseCases.UpdateTeacher;

// Request для обновления учителя
public class UpdateTeacherRequest
{
    public Guid Id { get; set; }
    public string? Specialization { get; set; }
    public int? PaymentType { get; set; }
    public decimal? HourlyRate { get; set; }
    public string? Bio { get; set; }
}
