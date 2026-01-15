namespace RazorWebApp.Models.Teachers;

// ViewModel учителя
public class TeacherViewModel
{
    public Guid Id { get; set; }
    public Guid UserId { get; set; }
    public string FullName { get; set; } = string.Empty;
    public string Phone { get; set; } = string.Empty;
    public string? Specialization { get; set; }
    public int PaymentType { get; set; }
    public decimal? HourlyRate { get; set; }
    public string? Bio { get; set; }
    public DateTime CreatedAt { get; set; }
    
    public string PaymentTypeName => PaymentType switch
    {
        0 => "Почасовая",
        1 => "За занятие",
        2 => "Оклад",
        _ => "Другое"
    };
}
