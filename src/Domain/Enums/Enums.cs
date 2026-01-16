namespace Domain.Enums;

// Статус группы
public enum GroupStatus
{
    Draft = 0,      // Черновик
    Active = 1,     // Активная
    Completed = 2,  // Завершена
    Cancelled = 3   // Отменена
}

// Статус зачисления
public enum EnrollmentStatus
{
    Active = 0,       // Учится
    Completed = 1,    // Закончил
    Expelled = 2,     // Отчислен
    Transferred = 3   // Переведён
}

// Тип занятия
public enum LessonType
{
    Regular = 0,      // Обычный урок
    Exam = 1,         // Экзамен
    Consultation = 2, // Консультация
    Makeup = 3,       // Отработка
    Replacement = 4   // Замена
}

// Статус занятия
public enum LessonStatus
{
    Planned = 0,     // Запланировано
    InProgress = 1,  // Идёт
    Completed = 2,   // Завершено
    Cancelled = 3    // Отменено
}

// Статус посещаемости
public enum AttendanceStatus
{
    Present = 0,    // Присутствовал
    Absent = 1,     // Отсутствовал
    Late = 2,       // Опоздал
    Excused = 3,    // Уважительная причина
    LeftEarly = 4   // Ушёл раньше
}

// Тип системы оценок
public enum GradingType
{
    Numeric = 0,   // Числовая (1-5, 0-100)
    Letter = 1,    // Буквенная (A-F)
    PassFail = 2   // Зачёт/Незачёт
}
