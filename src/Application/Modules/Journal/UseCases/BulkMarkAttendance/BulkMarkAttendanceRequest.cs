namespace Application.Modules.Journal.UseCases.BulkMarkAttendance;

// Запрос массовой отметки посещаемости
public class BulkMarkAttendanceRequest
{
    public Guid LessonId { get; set; }
    public List<BulkAttendanceItem> Items { get; set; } = new();
}
