namespace Domain.Schedule;

// Статус занятия
public enum LessonStatus
{
    Planned = 0,     // Запланировано
    InProgress = 1,  // Идёт
    Completed = 2,   // Завершено
    Cancelled = 3    // Отменено
}
