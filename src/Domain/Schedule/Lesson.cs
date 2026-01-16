using Domain.Common;
using Domain.Courses;
using Domain.Groups;
using Domain.Schedule;
using Domain.Journal;
using Domain.Grading;
using Domain.Groups;
using Domain.Journal;
using Domain.Rooms;
using Domain.Teachers;

namespace Domain.Schedule;

// Конкретное занятие
public class Lesson : BaseEntity
{
    public Guid GroupId { get; set; }
    public Group Group { get; set; } = null!;
    
    // Из какого шаблона создано (null = ручное)
    public Guid? ScheduleTemplateId { get; set; }
    public ScheduleTemplate? ScheduleTemplate { get; set; }
    
    // Дата и время
    public DateTime Date { get; set; }
    public TimeSpan StartTime { get; set; }
    public TimeSpan EndTime { get; set; }
    
    // Кабинет
    public Guid? RoomId { get; set; }
    public Room? Room { get; set; }
    
    // Учитель который РЕАЛЬНО ведёт
    public Guid TeacherId { get; set; }
    public Teacher Teacher { get; set; } = null!;
    
    // Тип и статус
    public LessonType Type { get; set; } = LessonType.Regular;
    public LessonStatus Status { get; set; } = LessonStatus.Planned;
    
    // Содержание
    public string? Topic { get; set; }
    public string? Description { get; set; }
    public string? Homework { get; set; }
    
    // Замена другого занятия
    public Guid? ReplacesLessonId { get; set; }
    public Lesson? ReplacesLesson { get; set; }
    public string? ReplacementReason { get; set; }
    
    // Если замена на другой курс (биология → физика)
    public Guid? OriginalCourseId { get; set; }
    public Course? OriginalCourse { get; set; }
    
    // Отмена/Перенос
    public string? CancellationReason { get; set; }
    public Guid? RescheduledToLessonId { get; set; }
    public Lesson? RescheduledToLesson { get; set; }
    
    // Связи
    public ICollection<LessonAssistant> Assistants { get; set; } = new List<LessonAssistant>();
    public ICollection<Attendance> Attendances { get; set; } = new List<Attendance>();
    public ICollection<Grade> Grades { get; set; } = new List<Grade>();
}
