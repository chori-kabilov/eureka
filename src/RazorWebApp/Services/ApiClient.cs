namespace RazorWebApp.Services;

// Базовый клиент для вызова API
public class ApiClient(HttpClient httpClient)
{
    public async Task<T?> GetAsync<T>(string url)
    {
        try
        {
            var response = await httpClient.GetAsync(url);
            if (!response.IsSuccessStatusCode)
                return default;
            return await response.Content.ReadFromJsonAsync<T>();
        }
        catch
        {
            return default;
        }
    }

    public async Task<TResponse?> PostAsync<TRequest, TResponse>(string url, TRequest data)
    {
        try
        {
            var response = await httpClient.PostAsJsonAsync(url, data);
            return await response.Content.ReadFromJsonAsync<TResponse>();
        }
        catch
        {
            return default;
        }
    }

    public async Task<TResponse?> PutAsync<TRequest, TResponse>(string url, TRequest data)
    {
        try
        {
            var response = await httpClient.PutAsJsonAsync(url, data);
            return await response.Content.ReadFromJsonAsync<TResponse>();
        }
        catch
        {
            return default;
        }
    }

    public async Task<TResponse?> PatchAsync<TRequest, TResponse>(string url, TRequest data)
    {
        try
        {
            Console.WriteLine($"PATCH {url}");
            var response = await httpClient.PatchAsJsonAsync(url, data);
            var content = await response.Content.ReadAsStringAsync();
            Console.WriteLine($"Response: {response.StatusCode} - {content}");
            return await response.Content.ReadFromJsonAsync<TResponse>();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"PATCH Error: {ex.Message}");
            return default;
        }
    }

    public async Task<bool> DeleteAsync(string url)
    {
        try
        {
            var response = await httpClient.DeleteAsync(url);
            return response.IsSuccessStatusCode;
        }
        catch
        {
            return false;
        }
    }
}
