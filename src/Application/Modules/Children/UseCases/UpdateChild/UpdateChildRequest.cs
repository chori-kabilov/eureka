namespace Application.Modules.Children.UseCases.UpdateChild;

// Request для обновления ребёнка
public class UpdateChildRequest
{
    public Guid Id { get; set; }
    public int? Status { get; set; }
    public string? Notes { get; set; }
}
