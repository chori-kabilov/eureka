using Application.Abstractions;
using Application.Common;
using Application.Modules.Children.Dtos;
using Application.Modules.Children.Mapping;
using Domain.Students;
using Microsoft.EntityFrameworkCore;

namespace Application.Modules.Children.UseCases.UpdateChild;

// Request для обновления ребёнка
public class UpdateChildRequest
{
    public Guid Id { get; set; }
    public int? Status { get; set; }
    public string? Notes { get; set; }
}

// Handler обновления ребёнка
public class UpdateChildHandler
{
    private readonly IDataContext _db;

    public UpdateChildHandler(IDataContext db)
    {
        _db = db;
    }

    public async Task<Result<ChildDetailDto>> HandleAsync(
        UpdateChildRequest request,
        CancellationToken ct = default)
    {
        var child = await _db.Children
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

        await _db.SaveChangesAsync(ct);

        return Result<ChildDetailDto>.Success(ChildMapper.ToDetailDto(child));
    }
}
