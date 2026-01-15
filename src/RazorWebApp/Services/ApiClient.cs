namespace RazorWebApp.Services;

// Базовый клиент для вызова API
public class ApiClient
{
    private readonly HttpClient _httpClient;

    public ApiClient(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<T?> GetAsync<T>(string url)
    {
        var response = await _httpClient.GetAsync(url);
        response.EnsureSuccessStatusCode();
        return await response.Content.ReadFromJsonAsync<T>();
    }

    public async Task<TResponse?> PostAsync<TRequest, TResponse>(string url, TRequest data)
    {
        var response = await _httpClient.PostAsJsonAsync(url, data);
        return await response.Content.ReadFromJsonAsync<TResponse>();
    }

    public async Task<TResponse?> PutAsync<TRequest, TResponse>(string url, TRequest data)
    {
        var response = await _httpClient.PutAsJsonAsync(url, data);
        return await response.Content.ReadFromJsonAsync<TResponse>();
    }

    public async Task<bool> DeleteAsync(string url)
    {
        var response = await _httpClient.DeleteAsync(url);
        return response.IsSuccessStatusCode;
    }

    // Установка JWT токена
    public void SetAuthToken(string token)
    {
        _httpClient.DefaultRequestHeaders.Authorization = 
            new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
    }

    public void ClearAuthToken()
    {
        _httpClient.DefaultRequestHeaders.Authorization = null;
    }
}
