namespace RazorWebApp.Models.Students;

// ViewModel студента
public class StudentViewModel
{
    public Guid Id { get; set; }
    public Guid UserId { get; set; }
    public string FullName { get; set; } = string.Empty;
    public string Phone { get; set; } = string.Empty;
    public decimal Balance { get; set; }
    public int Status { get; set; }
    public DateTime CreatedAt { get; set; }
    public string? Notes { get; set; }
    
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
}
