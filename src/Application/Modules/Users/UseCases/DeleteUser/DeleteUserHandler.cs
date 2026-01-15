using Application.Abstractions;
using Application.Common;
using Microsoft.EntityFrameworkCore;

namespace Application.Modules.Users.UseCases.DeleteUser;

// Handler удаления пользователя (soft delete)
public class DeleteUserHandler
{
    private readonly IDataContext _db;
    private readonly ICurrentUser _currentUser;

    public DeleteUserHandler(IDataContext db, ICurrentUser currentUser)
    {
        _db = db;
        _currentUser = currentUser;
    }

    public async Task<Result> HandleAsync(Guid id, CancellationToken ct = default)
    {
        var user = await _db.Users.FirstOrDefaultAsync(u => u.Id == id, ct);

        if (user == null)
            return Result.Failure(Error.NotFound("Пользователь"));

        // Защита: нельзя удалить себя
        if (_currentUser.UserId == user.Id)
            return Result.Failure(new Error("SELF_DELETE", "Нельзя удалить себя"));

        _db.Remove(user);
        await _db.SaveChangesAsync(ct);

        return Result.Success();
    }
}
