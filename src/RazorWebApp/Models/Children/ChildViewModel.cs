namespace RazorWebApp.Models.Children;

// ViewModel ребёнка
public class ChildViewModel
{
    public Guid Id { get; set; }
    public Guid ParentId { get; set; }
    public string ParentName { get; set; } = string.Empty;
    public string FullName { get; set; } = string.Empty;
    public DateTime? BirthDate { get; set; }
    public int Status { get; set; }
    public string? Notes { get; set; }
    public DateTime CreatedAt { get; set; }
    
    public string StatusName => Status switch
    {
        0 => "Активный",
        1 => "Неактивный",
        2 => "Закончил",
        3 => "Отчислен",
        _ => "Неизвестно"
    };
    
    public string StatusBadgeClass => Status switch
    {
        0 => "bg-success",
        1 => "bg-secondary",
        2 => "bg-info",
        3 => "bg-danger",
        _ => "bg-secondary"
    };
    
    public int? Age
    {
        get
        {
            if (!BirthDate.HasValue) return null;
            var today = DateTime.Today;
            var age = today.Year - BirthDate.Value.Year;
            if (BirthDate.Value.Date > today.AddYears(-age)) age--;
            return age;
        }
    }
}
