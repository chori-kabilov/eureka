using Application.Abstractions;
using Application.Common;
using Application.Modules.Rooms.Dtos;
using Application.Modules.Rooms.Mapping;
using Domain.Rooms;
using Microsoft.EntityFrameworkCore;

namespace Application.Modules.Rooms.UseCases;

// Получить список кабинетов
public class ListRoomsHandler
{
    private readonly IDataContext _db;

    public ListRoomsHandler(IDataContext db) => _db = db;

    public async Task<Result<List<RoomDto>>> HandleAsync(bool? activeOnly = true, CancellationToken ct = default)
    {
        var query = _db.Rooms.AsQueryable();
        
        if (activeOnly == true)
            query = query.Where(r => r.IsActive);

        var rooms = await query
            .OrderBy(r => r.Name)
            .ToListAsync(ct);

        return Result<List<RoomDto>>.Success(rooms.Select(RoomMapper.ToDto).ToList());
    }
}

// Создать кабинет
public class CreateRoomRequest
{
    public string Name { get; set; } = string.Empty;
    public string? Code { get; set; }
    public int? Capacity { get; set; }
    public string? Description { get; set; }
}

public class CreateRoomHandler
{
    private readonly IDataContext _db;

    public CreateRoomHandler(IDataContext db) => _db = db;

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

        _db.Add(room);
        await _db.SaveChangesAsync(ct);

        return Result<RoomDto>.Success(RoomMapper.ToDto(room));
    }
}

// Обновить кабинет
public class UpdateRoomRequest
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string? Code { get; set; }
    public int? Capacity { get; set; }
    public string? Description { get; set; }
    public bool IsActive { get; set; }
}

public class UpdateRoomHandler
{
    private readonly IDataContext _db;

    public UpdateRoomHandler(IDataContext db) => _db = db;

    public async Task<Result<RoomDto>> HandleAsync(UpdateRoomRequest request, CancellationToken ct = default)
    {
        var room = await _db.Rooms.FirstOrDefaultAsync(r => r.Id == request.Id, ct);
        if (room == null)
            return Result<RoomDto>.Failure(Error.NotFound("Кабинет"));

        room.Name = request.Name.Trim();
        room.Code = request.Code?.Trim();
        room.Capacity = request.Capacity;
        room.Description = request.Description?.Trim();
        room.IsActive = request.IsActive;

        await _db.SaveChangesAsync(ct);

        return Result<RoomDto>.Success(RoomMapper.ToDto(room));
    }
}

// Удалить кабинет
public class DeleteRoomHandler
{
    private readonly IDataContext _db;

    public DeleteRoomHandler(IDataContext db) => _db = db;

    public async Task<Result<bool>> HandleAsync(Guid id, CancellationToken ct = default)
    {
        var room = await _db.Rooms.FirstOrDefaultAsync(r => r.Id == id, ct);
        if (room == null)
            return Result<bool>.Failure(Error.NotFound("Кабинет"));

        _db.Remove(room);
        await _db.SaveChangesAsync(ct);

        return Result<bool>.Success(true);
    }
}
