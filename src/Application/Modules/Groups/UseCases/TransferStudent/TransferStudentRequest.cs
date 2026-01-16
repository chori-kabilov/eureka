namespace Application.Modules.Groups.UseCases.TransferStudent;

// Запрос на перевод в другую группу
public class TransferStudentRequest
{
    public Guid EnrollmentId { get; set; }
    public Guid NewGroupId { get; set; }
    public string? Notes { get; set; }
}
