using Application.Abstractions;
using Application.Common;
using Application.Modules.Rooms.Dtos;
using Application.Modules.Rooms.Mapping;
using Microsoft.EntityFrameworkCore;

namespace Application.Modules.Rooms.UseCases.UpdateRoom;

// Обновить кабинет
public class UpdateRoomHandler(IDataContext db)
{
    public async Task<Result<RoomDto>> HandleAsync(UpdateRoomRequest request, CancellationToken ct = default)
    {
        var room = await db.Rooms.FirstOrDefaultAsync(r => r.Id == request.Id, ct);
        if (room == null)
            return Result<RoomDto>.Failure(Error.NotFound("Кабинет"));

        room.Name = request.Name.Trim();
        room.Code = request.Code?.Trim();
        room.Capacity = request.Capacity;
        room.Description = request.Description?.Trim();
        room.IsActive = request.IsActive;

        await db.SaveChangesAsync(ct);

        return Result<RoomDto>.Success(RoomMapper.ToDto(room));
    }
}
