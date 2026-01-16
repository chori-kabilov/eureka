using Domain.Common;
using Domain.Enums;
using Domain.Groups;
using Domain.Rooms;

namespace Domain.Schedule;

// Шаблон расписания группы
public class ScheduleTemplate : BaseEntity
{
    public Guid GroupId { get; set; }
    public Group Group { get; set; } = null!;
    
    public DayOfWeek DayOfWeek { get; set; }
    public TimeSpan StartTime { get; set; }
    public TimeSpan EndTime { get; set; }
    
    public Guid? RoomId { get; set; }
    public Room? Room { get; set; }
    
    public LessonType DefaultLessonType { get; set; } = LessonType.Regular;
    public bool IsActive { get; set; } = true;
    
    // Период действия шаблона
    public DateTime? ValidFrom { get; set; }
    public DateTime? ValidTo { get; set; }
}
