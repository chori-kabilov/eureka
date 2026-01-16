namespace RazorWebApp.Services;

// ViewModel для кабинета
public class RoomViewModel
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string? Code { get; set; }
    public int? Capacity { get; set; }
    public bool IsActive { get; set; }
}
