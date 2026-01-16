using Domain.Schedule;

namespace Application.Modules.Schedule.UseCases.ListLessons;

// Запрос списка занятий
public class ListLessonsRequest
{
    public Guid? GroupId { get; set; }
    public Guid? TeacherId { get; set; }
    public DateTime? DateFrom { get; set; }
    public DateTime? DateTo { get; set; }
    public LessonStatus? Status { get; set; }
    public int Page { get; set; } = 1;
    public int PageSize { get; set; } = 50;
}
