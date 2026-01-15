namespace RazorWebApp.Models.Parents;

// ViewModel родителя
public class ParentViewModel
{
    public Guid Id { get; set; }
    public Guid UserId { get; set; }
    public string FullName { get; set; } = string.Empty;
    public string Phone { get; set; } = string.Empty;
    public int ChildrenCount { get; set; }
    public DateTime CreatedAt { get; set; }
}
