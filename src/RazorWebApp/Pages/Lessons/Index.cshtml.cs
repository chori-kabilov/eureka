using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.RazorPages;
using RazorWebApp.Services;

namespace RazorWebApp.Pages.Lessons;

[Authorize(Roles = "Admin")]
public class IndexModel : PageModel
{
    private readonly GroupsService _groupsService;
    private readonly ApiClient _apiClient;

    public IndexModel(GroupsService groupsService, ApiClient apiClient)
    {
        _groupsService = groupsService;
        _apiClient = apiClient;
    }

    public List<GroupItem> Groups { get; set; } = new();
    public List<LessonViewModel> Lessons { get; set; } = new();
    
    public Guid? GroupId { get; set; }
    public DateTime? DateFrom { get; set; }
    public DateTime? DateTo { get; set; }
    public string? Status { get; set; }
    public int CurrentPage { get; set; } = 1;
    public int PageSize { get; set; } = 20;
    public int TotalCount { get; set; }
    public int TotalPages { get; set; }

    public async Task OnGetAsync(Guid? groupId, DateTime? dateFrom, DateTime? dateTo, string? status, int page = 1)
    {
        GroupId = groupId;
        DateFrom = dateFrom;
        DateTo = dateTo;
        Status = status;
        CurrentPage = page;

        // Группы для фильтра
        var groupsResponse = await _groupsService.ListAsync();
        Groups = groupsResponse?.Items?.Select(g => new GroupItem
        {
            Id = g.Id,
            Name = g.Name
        }).ToList() ?? new();

        // Строим URL с параметрами
        var url = $"/api/v1/lessons?page={CurrentPage}&pageSize={PageSize}";
        if (groupId.HasValue) url += $"&groupId={groupId}";
        if (dateFrom.HasValue) url += $"&dateFrom={dateFrom.Value:yyyy-MM-dd}";
        if (dateTo.HasValue) url += $"&dateTo={dateTo.Value:yyyy-MM-dd}";
        if (!string.IsNullOrEmpty(status)) url += $"&status={status}";

        var response = await _apiClient.GetAsync<LessonsPageResponse>(url);
        
        if (response != null)
        {
            TotalCount = response.TotalCount;
            TotalPages = response.TotalPages;
            Lessons = response.Items?.Select(l => new LessonViewModel
            {
                Id = l.Id,
                GroupId = l.GroupId,
                GroupName = l.GroupName ?? "",
                Date = l.Date,
                StartTime = l.StartTime,
                EndTime = l.EndTime,
                TeacherName = l.TeacherName,
                RoomName = l.RoomName,
                Status = l.Status ?? "Planned",
                Topic = l.Topic
            }).ToList() ?? new();
        }
    }
}

public class GroupItem
{
    public Guid Id { get; set; }
    public string Name { get; set; } = "";
}

public class LessonViewModel
{
    public Guid Id { get; set; }
    public Guid GroupId { get; set; }
    public string GroupName { get; set; } = "";
    public DateTime Date { get; set; }
    public TimeSpan StartTime { get; set; }
    public TimeSpan EndTime { get; set; }
    public string? TeacherName { get; set; }
    public string? RoomName { get; set; }
    public string Status { get; set; } = "";
    public string? Topic { get; set; }
}

public class LessonsPageResponse
{
    public List<LessonApiItem>? Items { get; set; }
    public int Page { get; set; }
    public int PageSize { get; set; }
    public int TotalCount { get; set; }
    public int TotalPages { get; set; }
}

public class LessonApiItem
{
    public Guid Id { get; set; }
    public Guid GroupId { get; set; }
    public string? GroupName { get; set; }
    public DateTime Date { get; set; }
    public TimeSpan StartTime { get; set; }
    public TimeSpan EndTime { get; set; }
    public string? TeacherName { get; set; }
    public string? RoomName { get; set; }
    public string? Status { get; set; }
    public string? Topic { get; set; }
}
