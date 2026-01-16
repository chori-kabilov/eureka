using Application.Abstractions;
using Application.Common;
using Application.Modules.Users.Dtos;
using Application.Modules.Users.Mapping;
using Microsoft.EntityFrameworkCore;

namespace Application.Modules.Users.UseCases.ListUsers;

// Handler получения списка пользователей
public class ListUsersHandler(IDataContext db)
{
    public async Task<Result<PagedResult<UserDto>>> HandleAsync(
        ListUsersRequest request,
        CancellationToken ct = default)
    {
        var query = db.Users
            .Include(u => u.AdminProfile)
            .Include(u => u.StudentProfile)
            .Include(u => u.TeacherProfile)
            .Include(u => u.ParentProfile)
            .AsQueryable();

        // Фильтр по поиску (телефон или имя)
        if (!string.IsNullOrWhiteSpace(request.Search))
        {
            var search = request.Search.ToLower();
            query = query.Where(u => 
                u.Phone.ToLower().Contains(search) ||
                u.FullName.ToLower().Contains(search));
        }

        // Фильтр по Admin
        if (request.IsAdmin.HasValue)
        {
            if (request.IsAdmin.Value)
                query = query.Where(u => u.AdminProfile != null);
            else
                query = query.Where(u => u.AdminProfile == null);
        }

        // Подсчёт
        var totalCount = await query.CountAsync(ct);

        // Пагинация
        var skip = (request.Page - 1) * request.PageSize;
        var users = await query
            .OrderByDescending(u => u.CreatedAt)
            .Skip(skip)
            .Take(request.PageSize)
            .ToListAsync(ct);

        var result = new PagedResult<UserDto>
        {
            Items = users.Select(UserMapper.ToDto).ToList(),
            Page = request.Page,
            PageSize = request.PageSize,
            TotalCount = totalCount
        };

        return Result<PagedResult<UserDto>>.Success(result);
    }
}
