namespace Application.Modules.Journal.UseCases.SetGrade;

// Запрос на оставление оценки
public class SetGradeRequest
{
    public Guid LessonId { get; set; }
    public Guid? StudentId { get; set; }
    public Guid? ChildId { get; set; }
    public decimal Score { get; set; }
    public decimal Weight { get; set; } = 1;
    public string? Comment { get; set; }
}
