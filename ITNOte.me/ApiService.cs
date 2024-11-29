using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization.Metadata;
using System.Threading.Tasks;

namespace ITNOte.me;

public class ApiService(HttpClient httpClient)
{
    public async Task<string?> PostAndGetToken<T>(string url, T data)
    {
        var json = JsonSerializer.Serialize(data);
        var content = new StringContent(json, Encoding.UTF8, "application/json");
        var response = await httpClient.PostAsync(url, content);
        return JsonSerializer.Deserialize<TokenResponse>(await response.Content.ReadAsStringAsync())?.token;
    }
    
    public async Task<T?> GetAsync<T>(string url)
    {
        var response = await httpClient.GetAsync(url);
        if (!response.IsSuccessStatusCode) return default;
        var json = await response.Content.ReadAsStringAsync();
        var options = new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            TypeInfoResolver = new DefaultJsonTypeInfoResolver()
        };
        return JsonSerializer.Deserialize<T>(json, options);
    }

    public async Task<bool> PostAsync<T>(string url, T data)
    {
        var json = JsonSerializer.Serialize(data);
        var content = new StringContent(json, Encoding.UTF8, "application/json");
        var response = await httpClient.PostAsync(url, content);
        return response.IsSuccessStatusCode;
    }

    public async Task<bool> PutAsync(string url)
    {
        var response = await httpClient.PutAsync(url, null);
        return response.IsSuccessStatusCode;
    }

    public async Task<bool> DeleteAsync(string url)
    {
        var response = await httpClient.DeleteAsync(url);
        return response.IsSuccessStatusCode;
    }
}
