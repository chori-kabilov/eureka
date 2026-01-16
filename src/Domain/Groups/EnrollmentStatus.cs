namespace Domain.Groups;

// Статус зачисления
public enum EnrollmentStatus
{
    Active = 0,       // Учится
    Completed = 1,    // Закончил
    Expelled = 2,     // Отчислен
    Transferred = 3   // Переведён
}
