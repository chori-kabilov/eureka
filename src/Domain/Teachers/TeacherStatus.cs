namespace Domain.Teachers;

// Статус учителя
public enum TeacherStatus
{
    Active = 0,     // Активный
    OnPause = 1,    // На паузе (отпуск/больничный)
    Finished = 2    // Завершил работу
}
