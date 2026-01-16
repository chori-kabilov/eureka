namespace RazorWebApp.Services;

// ViewModel для системы оценок
public class GradingSystemViewModel
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public int Type { get; set; }
    public decimal MinScore { get; set; }
    public decimal MaxScore { get; set; }
    public decimal PassingScore { get; set; }
    public bool IsDefault { get; set; }

    public string TypeName => Type switch
    {
        0 => "Цифровая",
        1 => "Буквенная",
        2 => "Зачёт/Незачёт",
        _ => "Другое"
    };
}
