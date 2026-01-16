using Application.Abstractions;
using Application.Common;
using Application.Modules.Rooms.Dtos;
using Application.Modules.Rooms.Mapping;
using Domain.Rooms;

namespace Application.Modules.Rooms.UseCases.CreateRoom;

// Создать кабинет
public class CreateRoomHandler(IDataContext db)
{
    public async Task<Result<RoomDto>> HandleAsync(CreateRoomRequest request, CancellationToken ct = default)
    {
        if (string.IsNullOrWhiteSpace(request.Name))
            return Result<RoomDto>.Failure(Error.Validation("Название кабинета обязательно"));

        var room = new Room
        {
            Id = Guid.NewGuid(),
            Name = request.Name.Trim(),
            Code = request.Code?.Trim(),
            Capacity = request.Capacity,
            Description = request.Description?.Trim(),
            IsActive = true
        };

        db.Add(room);
        await db.SaveChangesAsync(ct);

        return Result<RoomDto>.Success(RoomMapper.ToDto(room));
    }
}
