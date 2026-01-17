using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using RazorWebApp.Models.Teachers;
using RazorWebApp.Services;

namespace RazorWebApp.Pages.Teachers;

[Authorize(Roles = "Admin")]
public class EditModel : PageModel
{
    private readonly TeachersService _teachersService;

    public EditModel(TeachersService teachersService)
    {
        _teachersService = teachersService;
    }

    public TeacherViewModel? Teacher { get; set; }

    [BindProperty]
    public string FullName { get; set; } = string.Empty;

    [BindProperty]
    public string Phone { get; set; } = string.Empty;

    [BindProperty]
    public int Status { get; set; }

    [BindProperty]
    public string Subjects { get; set; } = string.Empty;

    [BindProperty]
    public int PaymentType { get; set; }

    [BindProperty]
    public decimal? HourlyRate { get; set; }

    [BindProperty]
    public string? Bio { get; set; }

    public string? SuccessMessage { get; set; }
    public string? ErrorMessage { get; set; }

    public async Task<IActionResult> OnGetAsync(Guid id)
    {
        var response = await _teachersService.GetAsync(id);
        
        if (response?.Success != true || response.Data == null)
            return RedirectToPage("/Teachers/Index");

        Teacher = response.Data;
        FullName = Teacher.FullName;
        Phone = Teacher.Phone;
        Status = Teacher.Status;
        Subjects = string.Join(", ", Teacher.Subjects);
        PaymentType = Teacher.PaymentType;
        HourlyRate = Teacher.HourlyRate;
        Bio = Teacher.Bio;
        
        return Page();
    }

    public async Task<IActionResult> OnPostAsync(Guid id)
    {
        var subjectsList = string.IsNullOrWhiteSpace(Subjects) 
            ? new List<string>() 
            : Subjects.Split(',', StringSplitOptions.RemoveEmptyEntries).Select(s => s.Trim()).ToList();
            
        var result = await _teachersService.UpdateAsync(id, FullName, Phone, Status, subjectsList, PaymentType, HourlyRate, Bio);

        if (result?.Success == true)
        {
            SuccessMessage = "Учитель успешно обновлён";
            Teacher = result.Data;
            return Page();
        }

        ErrorMessage = result?.Error ?? "Ошибка при обновлении";
        
        var response = await _teachersService.GetAsync(id);
        Teacher = response?.Data;
        
        return Page();
    }
}
