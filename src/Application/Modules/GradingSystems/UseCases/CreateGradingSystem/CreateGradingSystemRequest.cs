using Domain.Grading;

namespace Application.Modules.GradingSystems.UseCases.CreateGradingSystem;

// Запрос на создание системы оценок
public class CreateGradingSystemRequest
{
    public string Name { get; set; } = string.Empty;
    public GradingType Type { get; set; } = GradingType.Numeric;
    public decimal MinScore { get; set; } = 1;
    public decimal MaxScore { get; set; } = 5;
    public decimal PassingScore { get; set; } = 3;
    public bool IsDefault { get; set; }
}
