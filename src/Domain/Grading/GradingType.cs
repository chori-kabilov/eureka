namespace Domain.Grading;

// Тип системы оценок
public enum GradingType
{
    Numeric = 0,   // Числовая (1-5, 0-100)
    Letter = 1,    // Буквенная (A-F)
    PassFail = 2   // Зачёт/Незачёт
}
