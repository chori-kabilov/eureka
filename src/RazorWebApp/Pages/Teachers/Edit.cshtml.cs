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
    public string? Specialization { get; set; }

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
        Specialization = Teacher.Specialization;
        PaymentType = Teacher.PaymentType;
        HourlyRate = Teacher.HourlyRate;
        Bio = Teacher.Bio;
        
        return Page();
    }

    public async Task<IActionResult> OnPostAsync(Guid id)
    {
        var result = await _teachersService.UpdateAsync(id, Specialization, PaymentType, HourlyRate, Bio);

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
