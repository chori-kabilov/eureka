using Application.Abstractions;
using Application.Common;
using Application.Modules.Children.Dtos;
using Application.Modules.Children.Mapping;
using Domain.Students;
using Microsoft.EntityFrameworkCore;

namespace Application.Modules.Children.UseCases.UpdateChild;

// Handler обновления ребёнка
public class UpdateChildHandler(IDataContext db)
{
    public async Task<Result<ChildDetailDto>> HandleAsync(
        UpdateChildRequest request,
        CancellationToken ct = default)
    {
        var child = await db.Children
            .Include(c => c.Parent)
                .ThenInclude(p => p!.User)
            .FirstOrDefaultAsync(c => c.Id == request.Id, ct);

        if (child == null)
            return Result<ChildDetailDto>.Failure(Error.NotFound("Ребёнок"));

        if (request.Status.HasValue)
            child.Status = (StudentStatus)request.Status.Value;
        
        if (request.Notes != null)
            child.Notes = request.Notes;

        child.UpdatedAt = DateTime.UtcNow;

        await db.SaveChangesAsync(ct);

        return Result<ChildDetailDto>.Success(ChildMapper.ToDetailDto(child));
    }
}
