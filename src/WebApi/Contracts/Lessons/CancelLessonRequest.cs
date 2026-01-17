namespace WebApi.Contracts.Lessons;

// Запрос на отмену занятия
public class CancelLessonRequest
{
    public string? Reason { get; set; }
}
