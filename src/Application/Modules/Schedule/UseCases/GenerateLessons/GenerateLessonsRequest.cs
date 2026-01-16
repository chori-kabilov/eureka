namespace Application.Modules.Schedule.UseCases.GenerateLessons;

// Запрос на генерацию занятий из шаблона
public class GenerateLessonsRequest
{
    public Guid GroupId { get; set; }
    public DateTime FromDate { get; set; }
    public DateTime ToDate { get; set; }
}
