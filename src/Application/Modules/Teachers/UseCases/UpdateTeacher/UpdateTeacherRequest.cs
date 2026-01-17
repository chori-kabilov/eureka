namespace Application.Modules.Teachers.UseCases.UpdateTeacher;

// Request обновления учителя
public class UpdateTeacherRequest
{
    public Guid Id { get; set; }
    public string? FullName { get; set; }
    public string? Phone { get; set; }
    public int? Status { get; set; }
    public List<string>? Subjects { get; set; }
    public int? PaymentType { get; set; }
    public decimal? HourlyRate { get; set; }
    public string? Bio { get; set; }
}
