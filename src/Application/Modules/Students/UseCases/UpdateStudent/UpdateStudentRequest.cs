namespace Application.Modules.Students.UseCases.UpdateStudent;

public class UpdateStudentRequest
{
    public Guid Id { get; set; }
    public string? FullName { get; set; }
    public string? Phone { get; set; }
    public int? Status { get; set; }
    public string? Notes { get; set; }
}
