using Domain.Students;
using Domain.Teachers;
using Domain.Users;

namespace Domain.Schedule;

// Ассистент занятия (учитель, студент или ребёнок)
public class LessonAssistant
{
    public Guid LessonId { get; set; }
    public Lesson Lesson { get; set; } = null!;
    
    // Если ассистент — учитель
    public Guid? TeacherId { get; set; }
    public Teacher? Teacher { get; set; }
    
    // Если ассистент — студент
    public Guid? StudentId { get; set; }
    public Student? Student { get; set; }
    
    // Если ассистент — ребёнок (редко)
    public Guid? ChildId { get; set; }
    public Child? Child { get; set; }
    
    public string? Role { get; set; }
}
