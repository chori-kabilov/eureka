using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.RazorPages;
using RazorWebApp.Services;

namespace RazorWebApp.Pages.Schedule;

[Authorize(Roles = "Admin")]
public class IndexModel : PageModel
{
    private readonly GroupsService _groupsService;
    private readonly LessonsService _lessonsService;
    private readonly ApiClient _apiClient;

    public IndexModel(GroupsService groupsService, LessonsService lessonsService, ApiClient apiClient)
    {
        _groupsService = groupsService;
        _lessonsService = lessonsService;
        _apiClient = apiClient;
    }

    public List<GroupSelectItem> Groups { get; set; } = new();
    public List<TeacherSelectItem> Teachers { get; set; } = new();
    public List<RoomItem> Rooms { get; set; } = new();
    public List<CalendarLesson> Lessons { get; set; } = new();
    public HashSet<string> OccupiedRoomIds { get; set; } = new();

    public async Task OnGetAsync()
    {
        // Получаем группы для фильтра
        var groupsResponse = await _groupsService.ListAsync();
        Groups = groupsResponse?.Items?.Select(g => new GroupSelectItem
        {
            Id = g.Id.ToString(),
            Name = g.Name
        }).ToList() ?? new();

        // Получаем занятия за текущий месяц
        var from = DateTime.Now.AddMonths(-1).ToString("yyyy-MM-dd");
        var to = DateTime.Now.AddMonths(2).ToString("yyyy-MM-dd");
        
        var lessonsResponse = await _apiClient.GetAsync<LessonsPagedResponse>(
            $"/api/v1/lessons?dateFrom={from}&dateTo={to}&pageSize=500");
        
        Lessons = lessonsResponse?.Items?.Select(l => new CalendarLesson
        {
            Id = l.Id.ToString(),
            GroupId = l.GroupId.ToString(),
            GroupName = l.GroupName ?? "",
            Date = l.Date.ToString("yyyy-MM-dd"),
            StartTime = l.StartTime.ToString(@"hh\:mm"),
            EndTime = l.EndTime.ToString(@"hh\:mm"),
            TeacherName = l.TeacherName ?? "",
            RoomName = l.RoomName ?? "",
            Status = l.Status ?? "Planned",
            Topic = l.Topic ?? ""
        }).ToList() ?? new();

        // Получаем уникальных преподавателей из занятий
        Teachers = Lessons
            .Where(l => !string.IsNullOrEmpty(l.TeacherName))
            .Select(l => l.TeacherName)
            .Distinct()
            .Select((name, index) => new TeacherSelectItem { Id = index.ToString(), Name = name })
            .ToList();

        // Получаем аудитории
        var roomsResponse = await _apiClient.GetAsync<RoomsResponse>("/api/v1/rooms");
        Rooms = roomsResponse?.Items?.Select(r => new RoomItem
        {
            Id = r.Id.ToString(),
            Name = r.Name,
            Capacity = r.Capacity
        }).ToList() ?? new();

        // Определяем занятые аудитории сейчас
        var now = DateTime.Now;
        var todayStr = now.ToString("yyyy-MM-dd");
        var currentTime = now.TimeOfDay;
        
        OccupiedRoomIds = Lessons
            .Where(l => l.Date == todayStr && 
                       TimeSpan.Parse(l.StartTime) <= currentTime && 
                       TimeSpan.Parse(l.EndTime) >= currentTime &&
                       !string.IsNullOrEmpty(l.RoomName))
            .Select(l => l.RoomName)
            .ToHashSet();
    }
}

// ViewModels
public class GroupSelectItem
{
    public string Id { get; set; } = "";
    public string Name { get; set; } = "";
}

public class TeacherSelectItem
{
    public string Id { get; set; } = "";
    public string Name { get; set; } = "";
}

public class RoomItem
{
    public string Id { get; set; } = "";
    public string Name { get; set; } = "";
    public int Capacity { get; set; }
}

public class CalendarLesson
{
    public string Id { get; set; } = "";
    public string GroupId { get; set; } = "";
    public string GroupName { get; set; } = "";
    public string Date { get; set; } = "";
    public string StartTime { get; set; } = "";
    public string EndTime { get; set; } = "";
    public string TeacherName { get; set; } = "";
    public string RoomName { get; set; } = "";
    public string Status { get; set; } = "";
    public string Topic { get; set; } = "";
}

public class LessonsPagedResponse
{
    public List<LessonItem>? Items { get; set; }
    public int TotalCount { get; set; }
}

public class LessonItem
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

public class RoomsResponse
{
    public List<RoomApiItem>? Items { get; set; }
}

public class RoomApiItem
{
    public Guid Id { get; set; }
    public string Name { get; set; } = "";
    public int Capacity { get; set; }
}
