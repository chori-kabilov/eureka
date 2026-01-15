namespace RazorWebApp.Models.Courses;

// ViewModel курса
public class CourseViewModel
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }
    public decimal Price { get; set; }
    public int DurationHours { get; set; }
    public int MaxStudents { get; set; }
    public bool IsArchived { get; set; }
    public DateTime CreatedAt { get; set; }
    
    public string StatusBadgeClass => IsArchived ? "bg-secondary" : "bg-success";
    public string StatusName => IsArchived ? "Архив" : "Активный";
}
