using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace RazorWebApp.Pages.Dashboard;

[Authorize]
public class IndexModel : PageModel
{
    public int CoursesCount { get; set; }

    public void OnGet()
    {
        // Пока статичные данные, позже подключим API
        CoursesCount = 0;
    }
}
