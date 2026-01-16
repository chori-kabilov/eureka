using Application.Abstractions;
using Application.Common;
using Microsoft.EntityFrameworkCore;

namespace Application.Modules.Users.UseCases.DeleteUser;

// Handler удаления пользователя (soft delete)
public class DeleteUserHandler(IDataContext db, ICurrentUser currentUser)
{
    public async Task<Result> HandleAsync(Guid id, CancellationToken ct = default)
    {
        var user = await db.Users.FirstOrDefaultAsync(u => u.Id == id, ct);

        if (user == null)
            return Result.Failure(Error.NotFound("Пользователь"));

        // Защита: нельзя удалить себя
        if (currentUser.UserId == user.Id)
            return Result.Failure(new Error("SELF_DELETE", "Нельзя удалить себя"));

        db.Remove(user);
        await db.SaveChangesAsync(ct);

        return Result.Success();
    }
}
