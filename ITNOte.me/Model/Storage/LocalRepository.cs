// using ITNotion.Notes;
using System.IO;
using System.Text.Json;
using static ITNOte.me.Model.ConfigurationSettings;

namespace ITNOte.me.Model.Storage;

public class LocalRepository : IStorage
{
    
    public async Task SaveRegistryUser<T>(T user)
    {
        if (!Directory.Exists(AppConfigurationSettings.KeyUserPath))
        {
            Directory.CreateDirectory(AppConfigurationSettings.KeyUserPath);
        }
        
        await using var createStream = File.Create($"{AppConfigurationSettings.KeyUserPath}{user}.json");
        await JsonSerializer.SerializeAsync(createStream, user);
    }
    
    
    public bool HasNicknameInStorage(string name)
    {
        return File.Exists($"{AppConfigurationSettings.KeyUserPath}{name}.json");
    }

    public async Task<T?> GetUserFromStorage<T>(string name)
    {
        await using var openStream = File.OpenRead($"{AppConfigurationSettings.KeyUserPath}{name}.json");
        return await JsonSerializer.DeserializeAsync<T>(openStream);
    }

    public async Task CreateNewSource(string path, string name)
    {
        if (!Directory.Exists(path))
        {
            Directory.CreateDirectory(path);
        }
    
        if (name.EndsWith(".txt"))
        {
            await using var createStream = File.Create($"{path}/{name}");
            return;
        }
        Directory.CreateDirectory($"{path}/{name}");
    }

    public async Task WriteInNote(string path, string name, string text)
    {
        await File.WriteAllTextAsync($"{path}/{name}.txt", text);
    }
}