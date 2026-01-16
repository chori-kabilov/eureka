namespace Application.Modules.Children.UseCases.CreateChild;

// Request для создания ребёнка
public class CreateChildRequest
{
    public Guid ParentId { get; set; }
    public string FullName { get; set; } = string.Empty;
    public DateTime? BirthDate { get; set; }
    public string? Notes { get; set; }
}
