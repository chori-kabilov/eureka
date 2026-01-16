using Domain.Common;
using Domain.Grading;
using Domain.Schedule;
using Domain.Students;
using Domain.Users;

namespace Domain.Journal;

// Оценка
public class Grade : BaseEntity
{
    public Guid LessonId { get; set; }
    public Lesson Lesson { get; set; } = null!;
    
    // Студент
    public Guid? StudentId { get; set; }
    public Student? Student { get; set; }
    
    // Ребёнок
    public Guid? ChildId { get; set; }
    public Child? Child { get; set; }
    
    // Система оценок
    public Guid GradingSystemId { get; set; }
    public GradingSystem GradingSystem { get; set; } = null!;
    
    public decimal Score { get; set; }
    public string? Letter { get; set; }
    
    // Вес оценки (экзамен = 2, урок = 1)
    public decimal Weight { get; set; } = 1;
    
    public string? Comment { get; set; }
    
    // Кто поставил
    public Guid GradedById { get; set; }
    public User GradedBy { get; set; } = null!;
    public DateTime GradedAt { get; set; }
}
