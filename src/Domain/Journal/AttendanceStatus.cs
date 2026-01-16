namespace Domain.Journal;

// Статус посещаемости
public enum AttendanceStatus
{
    Present = 0,    // Присутствовал
    Absent = 1,     // Отсутствовал
    Late = 2,       // Опоздал
    Excused = 3,    // Уважительная причина
    LeftEarly = 4   // Ушёл раньше
}
