using Domain.Common;

namespace Domain.Grading;

// Уровень для буквенной системы оценок
public class GradingLevel : BaseEntity
{
    public Guid GradingSystemId { get; set; }
    public GradingSystem GradingSystem { get; set; } = null!;
    
    public string Letter { get; set; } = string.Empty;
    public decimal MinScore { get; set; }
    public decimal MaxScore { get; set; }
    public string? Description { get; set; }
    public int Order { get; set; }
}
