using Application.Abstractions;
using Application.Common;
using Application.Modules.Groups.Dtos;
using Application.Modules.Groups.Mapping;
using Domain.Enums;
using Domain.Groups;
using Microsoft.EntityFrameworkCore;

namespace Application.Modules.Groups.UseCases;

// Запрос списка групп
public class ListGroupsRequest
{
    public string? Search { get; set; }
    public Guid? CourseId { get; set; }
    public Guid? TeacherId { get; set; }
    public GroupStatus? Status { get; set; }
    public int Page { get; set; } = 1;
    public int PageSize { get; set; } = 20;
}

// Получить список групп
public class ListGroupsHandler
{
    private readonly IDataContext _db;

    public ListGroupsHandler(IDataContext db) => _db = db;

    public async Task<Result<PagedResult<GroupDto>>> HandleAsync(ListGroupsRequest request, CancellationToken ct = default)
    {
        var query = _db.Groups
            .Include(g => g.Course)
            .Include(g => g.ResponsibleTeacher).ThenInclude(t => t.User)
            .AsQueryable();

        // Фильтры
        if (!string.IsNullOrWhiteSpace(request.Search))
        {
            var search = request.Search.ToLower();
            query = query.Where(g => g.Name.ToLower().Contains(search) || 
                                     (g.Code != null && g.Code.ToLower().Contains(search)));
        }

        if (request.CourseId.HasValue)
            query = query.Where(g => g.CourseId == request.CourseId);

        if (request.TeacherId.HasValue)
            query = query.Where(g => g.ResponsibleTeacherId == request.TeacherId || 
                                     g.DefaultTeacherId == request.TeacherId);

        if (request.Status.HasValue)
            query = query.Where(g => g.Status == request.Status);

        var totalCount = await query.CountAsync(ct);
        var skip = (request.Page - 1) * request.PageSize;

        var groups = await query
            .OrderByDescending(g => g.CreatedAt)
            .Skip(skip)
            .Take(request.PageSize)
            .ToListAsync(ct);

        // Подсчёт студентов для каждой группы
        var groupIds = groups.Select(g => g.Id).ToList();
        var studentCounts = await _db.GroupEnrollments
            .Where(e => groupIds.Contains(e.GroupId) && e.Status == EnrollmentStatus.Active)
            .GroupBy(e => e.GroupId)
            .Select(g => new { GroupId = g.Key, Count = g.Count() })
            .ToDictionaryAsync(x => x.GroupId, x => x.Count, ct);

        var items = groups.Select(g => GroupMapper.ToDto(g, studentCounts.GetValueOrDefault(g.Id, 0))).ToList();

        return Result<PagedResult<GroupDto>>.Success(new PagedResult<GroupDto>
        {
            Items = items,
            Page = request.Page,
            PageSize = request.PageSize,
            TotalCount = totalCount
        });
    }
}

// Получить группу по ID
public class GetGroupHandler
{
    private readonly IDataContext _db;

    public GetGroupHandler(IDataContext db) => _db = db;

    public async Task<Result<GroupDetailDto>> HandleAsync(Guid id, CancellationToken ct = default)
    {
        var group = await _db.Groups
            .Include(g => g.Course)
            .Include(g => g.ResponsibleTeacher).ThenInclude(t => t.User)
            .Include(g => g.DefaultTeacher).ThenInclude(t => t!.User)
            .Include(g => g.DefaultRoom)
            .Include(g => g.GradingSystem)
            .FirstOrDefaultAsync(g => g.Id == id, ct);

        if (group == null)
            return Result<GroupDetailDto>.Failure(Error.NotFound("Группа"));

        var studentCount = await _db.GroupEnrollments
            .CountAsync(e => e.GroupId == id && e.Status == EnrollmentStatus.Active, ct);

        return Result<GroupDetailDto>.Success(GroupMapper.ToDetailDto(group, studentCount));
    }
}

// Создать группу
public class CreateGroupRequest
{
    public string Name { get; set; } = string.Empty;
    public string? Code { get; set; }
    public Guid CourseId { get; set; }
    public Guid ResponsibleTeacherId { get; set; }
    public Guid? DefaultTeacherId { get; set; }
    public Guid? DefaultRoomId { get; set; }
    public Guid? GradingSystemId { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime? EndDate { get; set; }
    public int MaxStudents { get; set; } = 15;
    public string? Notes { get; set; }
}

public class CreateGroupHandler
{
    private readonly IDataContext _db;

    public CreateGroupHandler(IDataContext db) => _db = db;

    public async Task<Result<GroupDetailDto>> HandleAsync(CreateGroupRequest request, CancellationToken ct = default)
    {
        if (string.IsNullOrWhiteSpace(request.Name))
            return Result<GroupDetailDto>.Failure(Error.Validation("Название группы обязательно"));

        // Проверка курса
        var course = await _db.Courses.FirstOrDefaultAsync(c => c.Id == request.CourseId, ct);
        if (course == null)
            return Result<GroupDetailDto>.Failure(Error.NotFound("Курс"));

        // Проверка учителя
        var teacher = await _db.Teachers.Include(t => t.User).FirstOrDefaultAsync(t => t.Id == request.ResponsibleTeacherId, ct);
        if (teacher == null)
            return Result<GroupDetailDto>.Failure(Error.NotFound("Учитель"));

        var group = new Group
        {
            Id = Guid.NewGuid(),
            Name = request.Name.Trim(),
            Code = request.Code?.Trim(),
            CourseId = request.CourseId,
            ResponsibleTeacherId = request.ResponsibleTeacherId,
            DefaultTeacherId = request.DefaultTeacherId ?? request.ResponsibleTeacherId,
            DefaultRoomId = request.DefaultRoomId,
            GradingSystemId = request.GradingSystemId,
            StartDate = DateTime.SpecifyKind(request.StartDate, DateTimeKind.Utc),
            EndDate = request.EndDate.HasValue ? DateTime.SpecifyKind(request.EndDate.Value, DateTimeKind.Utc) : null,
            MaxStudents = request.MaxStudents,
            Status = GroupStatus.Draft,
            Notes = request.Notes
        };

        _db.Add(group);
        await _db.SaveChangesAsync(ct);

        // Загрузка связей
        group.Course = course;
        group.ResponsibleTeacher = teacher;

        return Result<GroupDetailDto>.Success(GroupMapper.ToDetailDto(group, 0));
    }
}

// Обновить группу
public class UpdateGroupRequest
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string? Code { get; set; }
    public Guid ResponsibleTeacherId { get; set; }
    public Guid? DefaultTeacherId { get; set; }
    public Guid? DefaultRoomId { get; set; }
    public Guid? GradingSystemId { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime? EndDate { get; set; }
    public int MaxStudents { get; set; }
    public GroupStatus Status { get; set; }
    public string? Notes { get; set; }
}

public class UpdateGroupHandler
{
    private readonly IDataContext _db;

    public UpdateGroupHandler(IDataContext db) => _db = db;

    public async Task<Result<GroupDetailDto>> HandleAsync(UpdateGroupRequest request, CancellationToken ct = default)
    {
        var group = await _db.Groups
            .Include(g => g.Course)
            .Include(g => g.ResponsibleTeacher).ThenInclude(t => t.User)
            .FirstOrDefaultAsync(g => g.Id == request.Id, ct);

        if (group == null)
            return Result<GroupDetailDto>.Failure(Error.NotFound("Группа"));

        group.Name = request.Name.Trim();
        group.Code = request.Code?.Trim();
        group.ResponsibleTeacherId = request.ResponsibleTeacherId;
        group.DefaultTeacherId = request.DefaultTeacherId;
        group.DefaultRoomId = request.DefaultRoomId;
        group.GradingSystemId = request.GradingSystemId;
        group.StartDate = DateTime.SpecifyKind(request.StartDate, DateTimeKind.Utc);
        group.EndDate = request.EndDate.HasValue ? DateTime.SpecifyKind(request.EndDate.Value, DateTimeKind.Utc) : null;
        group.MaxStudents = request.MaxStudents;
        group.Status = request.Status;
        group.Notes = request.Notes;

        await _db.SaveChangesAsync(ct);

        var studentCount = await _db.GroupEnrollments
            .CountAsync(e => e.GroupId == group.Id && e.Status == EnrollmentStatus.Active, ct);

        return Result<GroupDetailDto>.Success(GroupMapper.ToDetailDto(group, studentCount));
    }
}

// Удалить группу
public class DeleteGroupHandler
{
    private readonly IDataContext _db;

    public DeleteGroupHandler(IDataContext db) => _db = db;

    public async Task<Result<bool>> HandleAsync(Guid id, CancellationToken ct = default)
    {
        var group = await _db.Groups.FirstOrDefaultAsync(g => g.Id == id, ct);
        if (group == null)
            return Result<bool>.Failure(Error.NotFound("Группа"));

        _db.Remove(group);
        await _db.SaveChangesAsync(ct);

        return Result<bool>.Success(true);
    }
}
