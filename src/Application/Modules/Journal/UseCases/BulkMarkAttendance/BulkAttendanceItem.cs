using Domain.Journal;

namespace Application.Modules.Journal.UseCases.BulkMarkAttendance;

// Элемент массовой отметки
public class BulkAttendanceItem
{
    public Guid? StudentId { get; set; }
    public Guid? ChildId { get; set; }
    public AttendanceStatus Status { get; set; }
}
