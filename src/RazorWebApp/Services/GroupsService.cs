using RazorWebApp.Models.Common;
using RazorWebApp.Models.Groups;

namespace RazorWebApp.Services;

// Сервис для работы с группами
public class GroupsService(ApiClient apiClient)
{
    public async Task<PagedResponse<GroupViewModel>?> ListAsync(
        string? search = null,
        Guid? courseId = null,
        int? status = null,
        int page = 1,
        int pageSize = 12)
    {
        var url = $"/api/v1/groups?page={page}&pageSize={pageSize}";
        
        if (!string.IsNullOrEmpty(search))
            url += $"&search={Uri.EscapeDataString(search)}";
        if (courseId.HasValue)
            url += $"&courseId={courseId}";
        if (status.HasValue)
            url += $"&status={status}";

        return await apiClient.GetAsync<PagedResponse<GroupViewModel>>(url);
    }

    public async Task<ApiResponse<GroupViewModel>?> GetAsync(Guid id)
    {
        return await apiClient.GetAsync<ApiResponse<GroupViewModel>>($"/api/v1/groups/{id}");
    }

    public async Task<ApiResponse<GroupViewModel>?> CreateAsync(
        string name, string? code, Guid courseId, Guid responsibleTeacherId,
        DateTime startDate, DateTime? endDate, int maxStudents, string? notes)
    {
        var request = new
        {
            Name = name,
            Code = code,
            CourseId = courseId,
            ResponsibleTeacherId = responsibleTeacherId,
            DefaultTeacherId = responsibleTeacherId,
            StartDate = startDate,
            EndDate = endDate,
            MaxStudents = maxStudents,
            Notes = notes
        };
        return await apiClient.PostAsync<object, ApiResponse<GroupViewModel>>("/api/v1/groups", request);
    }

    public async Task<ApiResponse<GroupViewModel>?> UpdateAsync(
        Guid id, string name, string? code, Guid responsibleTeacherId,
        DateTime startDate, DateTime? endDate, int maxStudents, int status, string? notes)
    {
        var request = new
        {
            Name = name,
            Code = code,
            ResponsibleTeacherId = responsibleTeacherId,
            DefaultTeacherId = responsibleTeacherId,
            StartDate = startDate,
            EndDate = endDate,
            MaxStudents = maxStudents,
            Status = status,
            Notes = notes
        };
        return await apiClient.PutAsync<object, ApiResponse<GroupViewModel>>($"/api/v1/groups/{id}", request);
    }

    public async Task<bool> DeleteAsync(Guid id)
    {
        return await apiClient.DeleteAsync($"/api/v1/groups/{id}");
    }

    // Студенты группы
    public async Task<ApiResponse<List<EnrollmentViewModel>>?> GetStudentsAsync(Guid groupId)
    {
        return await apiClient.GetAsync<ApiResponse<List<EnrollmentViewModel>>>($"/api/v1/groups/{groupId}/students");
    }

    public async Task<ApiResponse<EnrollmentViewModel>?> EnrollAsync(Guid groupId, Guid? studentId, Guid? childId)
    {
        var request = new { StudentId = studentId, ChildId = childId };
        return await apiClient.PostAsync<object, ApiResponse<EnrollmentViewModel>>($"/api/v1/groups/{groupId}/students", request);
    }

    public async Task<bool> UnenrollAsync(Guid groupId, Guid enrollmentId)
    {
        return await apiClient.DeleteAsync($"/api/v1/groups/{groupId}/students/{enrollmentId}");
    }
}
