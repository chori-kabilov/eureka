using Domain.Common;
using Domain.Courses;
using Domain.Groups;
using Domain.Schedule;
using Domain.Journal;
using Domain.Grading;
using Domain.Grading;
using Domain.Rooms;
using Domain.Schedule;
using Domain.Teachers;

namespace Domain.Groups;

// Учебная группа
public class Group : BaseEntity
{
    public string Name { get; set; } = string.Empty;
    public string? Code { get; set; }
    
    // Курс
    public Guid CourseId { get; set; }
    public Course Course { get; set; } = null!;
    
    // Ответственный учитель (административно)
    public Guid ResponsibleTeacherId { get; set; }
    public Teacher ResponsibleTeacher { get; set; } = null!;
    
    // Учитель по умолчанию (обычно ведёт)
    public Guid? DefaultTeacherId { get; set; }
    public Teacher? DefaultTeacher { get; set; }
    
    // Кабинет по умолчанию
    public Guid? DefaultRoomId { get; set; }
    public Room? DefaultRoom { get; set; }
    
    // Система оценок
    public Guid? GradingSystemId { get; set; }
    public GradingSystem? GradingSystem { get; set; }
    
    // Период обучения
    public DateTime StartDate { get; set; }
    public DateTime? EndDate { get; set; }
    
    public int MaxStudents { get; set; } = 15;
    public GroupStatus Status { get; set; } = GroupStatus.Draft;
    public string? Notes { get; set; }
    
    // Связи
    public ICollection<GroupEnrollment> Enrollments { get; set; } = new List<GroupEnrollment>();
    public ICollection<ScheduleTemplate> ScheduleTemplates { get; set; } = new List<ScheduleTemplate>();
    public ICollection<Lesson> Lessons { get; set; } = new List<Lesson>();
}
