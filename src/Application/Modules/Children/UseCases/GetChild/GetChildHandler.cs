using Application.Abstractions;
using Application.Common;
using Application.Modules.Children.Dtos;
using Application.Modules.Children.Mapping;
using Microsoft.EntityFrameworkCore;

namespace Application.Modules.Children.UseCases.GetChild;

// Handler получения ребёнка по ID
public class GetChildHandler(IDataContext db)
{
    public async Task<Result<ChildDetailDto>> HandleAsync(Guid id, CancellationToken ct = default)
    {
        var child = await db.Children
            .Include(c => c.Parent)
                .ThenInclude(p => p!.User)
            .FirstOrDefaultAsync(c => c.Id == id, ct);

        if (child == null)
            return Result<ChildDetailDto>.Failure(Error.NotFound("Ребёнок"));

        return Result<ChildDetailDto>.Success(ChildMapper.ToDetailDto(child));
    }
}
