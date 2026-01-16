using Application.Abstractions;
using Application.Common;
using Application.Modules.Children.Dtos;
using Application.Modules.Children.Mapping;
using Microsoft.EntityFrameworkCore;

namespace Application.Modules.Children.UseCases.ListChildren;

// Handler получения списка детей
public class ListChildrenHandler(IDataContext db)
{
    public async Task<Result<PagedResult<ChildDto>>> HandleAsync(
        ListChildrenRequest request,
        CancellationToken ct = default)
    {
        var query = db.Children
            .Include(c => c.Parent)
                .ThenInclude(p => p.User)
            .AsQueryable();

        // Фильтр по родителю
        if (request.ParentId.HasValue)
        {
            query = query.Where(c => c.ParentId == request.ParentId.Value);
        }

        // Фильтр по поиску
        if (!string.IsNullOrWhiteSpace(request.Search))
        {
            var search = request.Search.ToLower();
            query = query.Where(c => c.FullName.ToLower().Contains(search));
        }

        // Подсчёт
        var totalCount = await query.CountAsync(ct);

        // Пагинация
        var skip = (request.Page - 1) * request.PageSize;
        var children = await query
            .OrderByDescending(c => c.CreatedAt)
            .Skip(skip)
            .Take(request.PageSize)
            .ToListAsync(ct);

        var result = new PagedResult<ChildDto>
        {
            Items = children.Select(ChildMapper.ToDto).ToList(),
            Page = request.Page,
            PageSize = request.PageSize,
            TotalCount = totalCount
        };

        return Result<PagedResult<ChildDto>>.Success(result);
    }
}
