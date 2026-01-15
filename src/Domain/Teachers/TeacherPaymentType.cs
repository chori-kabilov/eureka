namespace Domain.Teachers;

// Тип оплаты учителя
public enum TeacherPaymentType
{
    Hourly = 0,      // Почасовая
    PerLesson = 1,   // За урок
    Monthly = 2,     // Ежемесячная
    PerStudent = 3   // За студента
}
