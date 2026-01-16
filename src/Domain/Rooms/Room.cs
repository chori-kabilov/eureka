using Domain.Common;

namespace Domain.Rooms;

// Кабинет/аудитория
public class Room : BaseEntity
{
    public string Name { get; set; } = string.Empty;
    public string? Code { get; set; }
    public int? Capacity { get; set; }
    public string? Description { get; set; }
    public bool IsActive { get; set; } = true;
}
