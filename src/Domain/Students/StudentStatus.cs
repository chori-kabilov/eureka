namespace Domain.Students;

// Статус студента
public enum StudentStatus
{
    Active = 0,      // Активный
    Inactive = 1,    // Неактивный (перерыв)
    Graduated = 2,   // Закончил обучение
    Expelled = 3     // Отчислен
}
