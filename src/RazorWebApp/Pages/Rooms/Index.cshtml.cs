using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using RazorWebApp.Services;

namespace RazorWebApp.Pages.Rooms;

[Authorize(Roles = "Admin")]
public class IndexModel : PageModel
{
    private readonly RoomsService _roomsService;

    public IndexModel(RoomsService roomsService)
    {
        _roomsService = roomsService;
    }

    public List<RoomViewModel> Rooms { get; set; } = new();

    public async Task OnGetAsync()
    {
        var response = await _roomsService.ListAsync();
        Rooms = response?.Data ?? new List<RoomViewModel>();
    }

    public async Task<IActionResult> OnPostCreateAsync(string name, string? code, int? capacity)
    {
        if (!string.IsNullOrWhiteSpace(name))
        {
            await _roomsService.CreateAsync(name, code, capacity);
        }
        return RedirectToPage();
    }

    public async Task<IActionResult> OnPostUpdateAsync(Guid id, string name, string? code, int? capacity, bool isActive = false)
    {
        if (!string.IsNullOrWhiteSpace(name))
        {
            await _roomsService.UpdateAsync(id, name, code, capacity, isActive);
        }
        return RedirectToPage();
    }

    public async Task<IActionResult> OnPostDeleteAsync(Guid id)
    {
        await _roomsService.DeleteAsync(id);
        return RedirectToPage();
    }
}
