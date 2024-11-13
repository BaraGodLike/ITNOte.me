using System.IO;
using System.Text.Json;
using ITNOte.me.Model.Notes;
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
        var options = new JsonSerializerOptions
        {
            Converters = { new AbstractSourceConverter() },
            WriteIndented = true,
            MaxDepth = 256
        };
        
        await using var createStream = File.Create($"{AppConfigurationSettings.KeyUserPath}{user}.json");
        await JsonSerializer.SerializeAsync(createStream, user, options);
    }
    
    
    public async Task<bool> HasNicknameInStorage(string name)
    {
        return File.Exists($"{AppConfigurationSettings.KeyUserPath}{name}.json");
    }

    public async Task<T?> GetUserFromStorage<T>(string name)
    {
        var options = new JsonSerializerOptions
        {
            Converters = { new AbstractSourceConverter() },
            WriteIndented = true,
            MaxDepth = 256
        };
        await using var openStream = File.OpenRead($"{AppConfigurationSettings.KeyUserPath}{name}.json");
        return await JsonSerializer.DeserializeAsync<T>(openStream, options);
    }

    public async Task CreateNewSource(AbstractSource source)
    {
        if (!Directory.Exists($"{AppConfigurationSettings.KeyUserSourcesPath}{source.Path}"))
        {
            Directory.CreateDirectory($"{AppConfigurationSettings.KeyUserSourcesPath}{source.Path}");
        }
    
        if (source.Type == nameof(Note))
        {   if (File.Exists($"{AppConfigurationSettings.KeyUserSourcesPath}{source.Path}/{source.Name}.txt"))
                return;
            await using var createStream = File.Create($"{AppConfigurationSettings.KeyUserSourcesPath}{source.Path}/{source.Name}.txt");
            return;
        }

        if (Directory.Exists($"{AppConfigurationSettings.KeyUserSourcesPath}{source.Path}/{source.Name}"))
            return;
        Directory.CreateDirectory($"{AppConfigurationSettings.KeyUserSourcesPath}{source.Path}/{source.Name}");
    }
    
    public async Task WriteInNote(int id, string name, string text)
    {
        await File.WriteAllTextAsync($"{AppConfigurationSettings.KeyUserSourcesPath}{id}/{name}.txt", text);
    }

    public async Task<string> ReadNote(int path, string name)
    {
        return await File.ReadAllTextAsync($"{AppConfigurationSettings.KeyUserSourcesPath}{path}/{name}.txt");
    }
}