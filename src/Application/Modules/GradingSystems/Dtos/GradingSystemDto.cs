using Domain.Grading;

namespace Application.Modules.GradingSystems.Dtos;

// DTO системы оценок
public class GradingSystemDto
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public GradingType Type { get; set; }
    public decimal MinScore { get; set; }
    public decimal MaxScore { get; set; }
    public decimal PassingScore { get; set; }
    public bool IsDefault { get; set; }
}
