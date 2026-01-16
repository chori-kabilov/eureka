using RazorWebApp.Models.Common;
using RazorWebApp.Pages.Groups;

namespace RazorWebApp.Services;

// Сервис для работы с журналом
public class JournalService(ApiClient apiClient)
{
    public async Task<ApiResponse<List<JournalRowViewModel>>?> GetGroupJournalAsync(
        Guid groupId, DateTime? dateFrom = null, DateTime? dateTo = null)
    {
        var url = $"/api/v1/journal/groups/{groupId}";
        
        if (dateFrom.HasValue)
            url += $"?dateFrom={dateFrom.Value:yyyy-MM-dd}";
        if (dateTo.HasValue)
            url += $"{(dateFrom.HasValue ? "&" : "?")}dateTo={dateTo.Value:yyyy-MM-dd}";

        return await apiClient.GetAsync<ApiResponse<List<JournalRowViewModel>>>(url);
    }
}
