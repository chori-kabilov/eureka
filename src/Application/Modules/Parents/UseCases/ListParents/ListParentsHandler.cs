using Application.Abstractions;
using Application.Common;
using Application.Modules.Parents.Dtos;
using Application.Modules.Parents.Mapping;
using Microsoft.EntityFrameworkCore;

namespace Application.Modules.Parents.UseCases.ListParents;

// Запрос на список родителей
public class ListParentsRequest
{
    public string? Search { get; set; }
    public int Page { get; set; } = 1;
    public int PageSize { get; set; } = 20;
}

// Handler получения списка родителей
public class ListParentsHandler
{
    private readonly IDataContext _db;

    public ListParentsHandler(IDataContext db)
    {
        _db = db;
    }

    public async Task<Result<PagedResult<ParentDto>>> HandleAsync(
        ListParentsRequest request,
        CancellationToken ct = default)
    {
        var query = _db.Parents
            .Include(p => p.User)
            .Include(p => p.Children)
            .AsQueryable();

        // Фильтр по поиску
        if (!string.IsNullOrWhiteSpace(request.Search))
        {
            var search = request.Search.ToLower();
            query = query.Where(p => 
                p.User!.Phone.ToLower().Contains(search) ||
                p.User.FullName.ToLower().Contains(search));
        }

        // Подсчёт
        var totalCount = await query.CountAsync(ct);

        // Пагинация
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
