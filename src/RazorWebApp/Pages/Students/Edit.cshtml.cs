using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using RazorWebApp.Models.Students;
using RazorWebApp.Services;

namespace RazorWebApp.Pages.Students;

[Authorize(Roles = "Admin")]
public class EditModel : PageModel
{
    private readonly StudentsService _studentsService;

    public EditModel(StudentsService studentsService)
    {
        _studentsService = studentsService;
    }

    public StudentViewModel? Student { get; set; }

[BindProperty]
    public int Status { get; set; }

    [BindProperty]
    public string? Notes { get; set; }

    [BindProperty]
    public string FullName { get; set; } = string.Empty;

    [BindProperty]
    public string Phone { get; set; } = string.Empty;

    public string? SuccessMessage { get; set; }
    public string? ErrorMessage { get; set; }

    public async Task<IActionResult> OnGetAsync(Guid id)
    {
        var response = await _studentsService.GetAsync(id);
        
        if (response?.Success != true || response.Data == null)
            return RedirectToPage("/Students/Index");

        Student = response.Data;
        Status = Student.Status;
        Notes = Student.Notes;
        FullName = Student.FullName;
        Phone = Student.Phone;
        
        return Page();
    }

    public async Task<IActionResult> OnPostAsync(Guid id)
    {
        var result = await _studentsService.UpdateAsync(id, FullName, Phone, Status, Notes);

        if (result?.Success == true)
        {
            SuccessMessage = "Студент успешно обновлён";
            Student = result.Data;
            return Page();
        }

        ErrorMessage = result?.Error ?? "Ошибка при обновлении";
        
        var response = await _studentsService.GetAsync(id);
        Student = response?.Data;
        
        return Page();
    }
}
