using Application.Abstractions;
using Application.Common;
using Application.Modules.Parents.Dtos;
using Application.Modules.Parents.Mapping;
using Microsoft.EntityFrameworkCore;

namespace Application.Modules.Parents.UseCases.ListParents;

// Получить список родителей
public class ListParentsHandler(IDataContext db)
{
    public async Task<Result<PagedResult<ParentDto>>> HandleAsync(
        ListParentsRequest request,
        CancellationToken ct = default)
    {
        var query = db.Parents
            .Include(p => p.User)
            .Include(p => p.Children)
            .AsQueryable();

        if (!string.IsNullOrWhiteSpace(request.Search))
        {
            var search = request.Search.ToLower();
            query = query.Where(p => 
                p.User!.Phone.ToLower().Contains(search) ||
                p.User.FullName.ToLower().Contains(search));
        }

        var totalCount = await query.CountAsync(ct);

        var skip = (request.Page - 1) * request.PageSize;
        var parents = await query
            .OrderByDescending(p => p.CreatedAt)
            .Skip(skip)
            .Take(request.PageSize)
            .ToListAsync(ct);

        var result = new PagedResult<ParentDto>
        {
            Items = parents.Select(ParentMapper.ToDto).ToList(),
            Page = request.Page,
            PageSize = request.PageSize,
            TotalCount = totalCount
        };

        return Result<PagedResult<ParentDto>>.Success(result);
    }
}
