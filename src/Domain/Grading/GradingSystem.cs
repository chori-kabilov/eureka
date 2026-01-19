using Domain.Common;

namespace Domain.Grading;

// Система оценок
public class GradingSystem : BaseEntity
{
    public string Name { get; set; } = string.Empty;
    public GradingType Type { get; set; } = GradingType.Numeric;
    public decimal MinScore { get; set; } = 1;
    public decimal MaxScore { get; set; } = 5;
    public decimal PassingScore { get; set; } = 3;
    public bool IsDefault { get; set; }
    
    public ICollection<GradingLevel>? Levels { get; set; }
}
