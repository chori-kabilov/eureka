namespace Application.Modules.Rooms.Dtos;

// DTO кабинета
public class RoomDto
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string? Code { get; set; }
    public int? Capacity { get; set; }
    public string? Description { get; set; }
    public bool IsActive { get; set; }
}
