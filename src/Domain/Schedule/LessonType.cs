namespace Domain.Schedule;

// Тип занятия
public enum LessonType
{
    Regular = 0,      // Обычный урок
    Exam = 1,         // Экзамен
    Consultation = 2, // Консультация
    Makeup = 3,       // Отработка
    Replacement = 4   // Замена
}
