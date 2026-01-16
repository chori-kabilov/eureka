using RazorWebApp.Models.Common;

namespace RazorWebApp.Services;

// Сервис для работы с системами оценок
public class GradingService(ApiClient apiClient)
{
    public async Task<List<GradingSystemViewModel>> ListAsync()
    {
        var response = await apiClient.GetAsync<List<GradingSystemViewModel>>("/api/v1/grading-systems");
        return response ?? new List<GradingSystemViewModel>();
    }

    public async Task<bool> CreateAsync(string name, int type, decimal minScore, decimal maxScore, decimal passingScore, bool isDefault)
    {
        var request = new
        {
            Name = name,
            Type = type,
            MinScore = minScore,
            MaxScore = maxScore,
            PassingScore = passingScore,
            IsDefault = isDefault
        };
        var result = await apiClient.PostAsync<object, ApiResponse<object>>("/api/v1/grading-systems", request);
        return result?.Success == true;
    }
}

