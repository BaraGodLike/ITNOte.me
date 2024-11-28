using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace ITNOte.me;

public class ApiService(HttpClient httpClient)
{
    public async Task<T?> GetAsync<T>(string url)
    {
        var response = await httpClient.GetAsync(url);
        if (response.IsSuccessStatusCode)
        {
            var json = await response.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<T>(json);
        }
        return default;
    }

    public async Task<bool> PostAsync<T>(string url, T data)
    {
        var json = JsonSerializer.Serialize(data);
        var content = new StringContent(json, Encoding.UTF8, "application/json");
        var response = await httpClient.PostAsync(url, content);
        return response.IsSuccessStatusCode;
    }

    public async Task<bool> PutAsync<T>(string url, T data)
    {
        var json = JsonSerializer.Serialize(data);
        var content = new StringContent(json, Encoding.UTF8, "application/json");
        var response = await httpClient.PutAsync(url, content);
        return response.IsSuccessStatusCode;
    }

    public async Task<bool> DeleteAsync(string url)
    {
        var response = await httpClient.DeleteAsync(url);
        return response.IsSuccessStatusCode;
    }
}
