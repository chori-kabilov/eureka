using Application.Abstractions;
using Application.Common;
using Application.Modules.Rooms.Dtos;
using Application.Modules.Rooms.Mapping;
using Microsoft.EntityFrameworkCore;

namespace Application.Modules.Rooms.UseCases.ListRooms;

// Получить список кабинетов
public class ListRoomsHandler(IDataContext db)
{
    public async Task<Result<List<RoomDto>>> HandleAsync(bool? activeOnly = true, CancellationToken ct = default)
    {
        var query = db.Rooms.AsQueryable();
        
        if (activeOnly == true)
            query = query.Where(r => r.IsActive);

        var rooms = await query
            .OrderBy(r => r.Name)
            .ToListAsync(ct);

        return Result<List<RoomDto>>.Success(rooms.Select(RoomMapper.ToDto).ToList());
    }
}
