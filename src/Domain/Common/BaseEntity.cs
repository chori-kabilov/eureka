namespace Domain.Common;

// Базовый класс для всех сущностей
// Объединяет: Id, аудит (Created/Updated), soft delete
public abstract class BaseEntity
{
    // Идентификатор
    public Guid Id { get; set; }

    // Аудит: кто и когда создал
    public DateTime CreatedAt { get; set; }
    public Guid? CreatedBy { get; set; }

    // Аудит: кто и когда изменил
    public DateTime? UpdatedAt { get; set; }
    public Guid? UpdatedBy { get; set; }

    // Soft delete
    public bool IsDeleted { get; set; }
    public DateTime? DeletedAt { get; set; }
}
