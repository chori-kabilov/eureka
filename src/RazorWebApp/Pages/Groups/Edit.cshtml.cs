using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using RazorWebApp.Models.Teachers;
using RazorWebApp.Services;

namespace RazorWebApp.Pages.Groups;

[Authorize(Roles = "Admin")]
public class EditModel : PageModel
{
    private readonly GroupsService _groupsService;
    private readonly TeachersService _teachersService;

    public EditModel(GroupsService groupsService, TeachersService teachersService)
    {
        _groupsService = groupsService;
        _teachersService = teachersService;
    }

    [BindProperty]
    public Guid Id { get; set; }

    [BindProperty]
    public string Name { get; set; } = string.Empty;

    [BindProperty]
    public string? Code { get; set; }

    [BindProperty]
    public Guid TeacherId { get; set; }

    [BindProperty]
    public DateTime StartDate { get; set; }

    [BindProperty]
    public DateTime? EndDate { get; set; }

    [BindProperty]
    public int MaxStudents { get; set; }

    [BindProperty]
    public int Status { get; set; }

    [BindProperty]
    public string? Notes { get; set; }

    public string CourseName { get; set; } = string.Empty;
    public List<TeacherViewModel> Teachers { get; set; } = new();
    public string? ErrorMessage { get; set; }

    public async Task<IActionResult> OnGetAsync(Guid id)
    {
        var response = await _groupsService.GetAsync(id);
        if (response?.Success != true || response.Data == null)
            return RedirectToPage("/Groups/Index");

        var group = response.Data;
        Id = group.Id;
        Name = group.Name;
        Code = group.Code;
        TeacherId = group.ResponsibleTeacherId;
        StartDate = group.StartDate;
        EndDate = group.EndDate;
        MaxStudents = group.MaxStudents;
        Status = group.Status;
        Notes = group.Notes;
        CourseName = group.CourseName;

        await LoadDataAsync();
        return Page();
    }

    public async Task<IActionResult> OnPostAsync()
    {
        if (string.IsNullOrWhiteSpace(Name))
        {
            ErrorMessage = "Введите название группы";
            await LoadDataAsync();
            return Page();
        }

        var result = await _groupsService.UpdateAsync(
            Id, Name, Code, TeacherId, StartDate, EndDate, MaxStudents, Status, Notes);

        if (result?.Success == true)
            return RedirectToPage("/Groups/Details", new { id = Id });

        ErrorMessage = result?.Error ?? "Не удалось обновить группу";
        await LoadDataAsync();
        return Page();
    }

    private async Task LoadDataAsync()
    {
        var teachersResult = await _teachersService.ListAsync();
        Teachers = teachersResult?.Items?.ToList() ?? new List<TeacherViewModel>();
    }
}
