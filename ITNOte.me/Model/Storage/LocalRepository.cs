using System.IO;
using System.Text.Json;
using static ITNOte.me.Model.ConfigurationSettings;

namespace ITNOte.me.Model.Storage;

public class LocalRepository : IStorage
{
    
    public async Task SaveUser<T>(T user)
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

    public async Task CreateNewSource(string path, string name, bool isFile)
    {
        if (!Directory.Exists($"{AppConfigurationSettings.KeyUserSourcesPath}{path}"))
        {
            Directory.CreateDirectory($"{AppConfigurationSettings.KeyUserSourcesPath}{path}");
        }
    
        if (isFile)
        {   if (File.Exists($"{AppConfigurationSettings.KeyUserSourcesPath}{path}/{name}.txt"))
                return;
            await using var createStream = File.Create($"{AppConfigurationSettings.KeyUserSourcesPath}{path}/{name}.txt");
            return;
        }

        if (Directory.Exists($"{AppConfigurationSettings.KeyUserSourcesPath}{path}/{name}"))
            return;
        Directory.CreateDirectory($"{AppConfigurationSettings.KeyUserSourcesPath}{path}/{name}");
    }

    public async Task WriteInNote(string path, string name, string text)
    {
        await File.WriteAllTextAsync($"{AppConfigurationSettings.KeyUserSourcesPath}{path}/{name}.txt", text);
    }

    public async Task<string> ReadNote(string path, string name)
    {
        return await File.ReadAllTextAsync($"{path}/{name}");
    }
}