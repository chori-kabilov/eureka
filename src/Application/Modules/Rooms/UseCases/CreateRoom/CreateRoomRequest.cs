namespace Application.Modules.Rooms.UseCases.CreateRoom;

// Запрос на создание кабинета
public class CreateRoomRequest
{
    public string Name { get; set; } = string.Empty;
    public string? Code { get; set; }
    public int? Capacity { get; set; }
    public string? Description { get; set; }
}
