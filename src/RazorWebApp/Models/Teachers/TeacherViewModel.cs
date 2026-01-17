namespace RazorWebApp.Models.Teachers;

// ViewModel учителя
public class TeacherViewModel
{
    public Guid Id { get; set; }
    public Guid UserId { get; set; }
    public string FullName { get; set; } = string.Empty;
    public string Phone { get; set; } = string.Empty;
    public int Status { get; set; }
    public List<string> Subjects { get; set; } = new();
    public int PaymentType { get; set; }
    public decimal? HourlyRate { get; set; }
    public string? Bio { get; set; }
    public DateTime? HiredAt { get; set; }
    public DateTime CreatedAt { get; set; }
    
    public string StatusName => Status switch
    {
        0 => "Активный",
        1 => "На паузе",
        2 => "Завершил работу",
        _ => "—"
    };

    public string StatusClass => Status switch
    {
        0 => "bg-success",
        1 => "bg-warning",
        2 => "bg-secondary",
        _ => "bg-secondary"
    };
    
    public string PaymentTypeName => PaymentType switch
    {
        0 => "Почасовая",
        1 => "За занятие",
        2 => "Оклад",
        _ => "Другое"
    };
    
    // Предметы в виде строки для отображения
    public string SubjectsDisplay => Subjects.Any() ? string.Join(", ", Subjects) : "—";
}
