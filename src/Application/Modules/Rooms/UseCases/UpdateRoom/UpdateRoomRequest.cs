namespace Application.Modules.Rooms.UseCases.UpdateRoom;

// Запрос на обновление кабинета
public class UpdateRoomRequest
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string? Code { get; set; }
    public int? Capacity { get; set; }
    public string? Description { get; set; }
    public bool IsActive { get; set; }
}
