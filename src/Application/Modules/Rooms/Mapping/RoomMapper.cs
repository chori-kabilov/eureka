using Application.Modules.Rooms.Dtos;
using Domain.Rooms;

namespace Application.Modules.Rooms.Mapping;

public static class RoomMapper
{
    public static RoomDto ToDto(Room room) => new()
    {
        Id = room.Id,
        Name = room.Name,
        Code = room.Code,
        Capacity = room.Capacity,
        Description = room.Description,
        IsActive = room.IsActive
    };
}
