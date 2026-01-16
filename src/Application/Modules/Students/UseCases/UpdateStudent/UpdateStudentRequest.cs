namespace Application.Modules.Students.UseCases.UpdateStudent;

// Request для обновления студента
public class UpdateStudentRequest
{
    public Guid Id { get; set; }
    public int? Status { get; set; }
    public string? Notes { get; set; }
}
