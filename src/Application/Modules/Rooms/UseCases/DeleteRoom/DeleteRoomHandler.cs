using Application.Abstractions;
using Application.Common;
using Microsoft.EntityFrameworkCore;

namespace Application.Modules.Rooms.UseCases.DeleteRoom;

// Удалить кабинет
public class DeleteRoomHandler(IDataContext db)
{
    public async Task<Result<bool>> HandleAsync(Guid id, CancellationToken ct = default)
    {
        var room = await db.Rooms.FirstOrDefaultAsync(r => r.Id == id, ct);
        if (room == null)
            return Result<bool>.Failure(Error.NotFound("Кабинет"));

        db.Remove(room);
        await db.SaveChangesAsync(ct);

        return Result<bool>.Success(true);
    }
}
