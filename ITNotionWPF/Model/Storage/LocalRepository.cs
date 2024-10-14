// using ITNotion.Notes;
using System.IO;
using System.Text.Json;
using static ITNotionWPF.Settings;

namespace ITNotionWPF.Model.Storage;

public class LocalRepository : IStorage
{
    
    public async Task SaveRegistryUser<T>(T user)
    {
        if (!Directory.Exists(AppSettings.KeyUserPath))
        {
            Directory.CreateDirectory(AppSettings.KeyUserPath);
        }
        
        await using var createStream = File.Create($"{AppSettings.KeyUserPath}{user}.json");
        await JsonSerializer.SerializeAsync(createStream, user);
    }
    
    
    public bool HasNicknameInStorage(string name)
    {
        return File.Exists($"{AppSettings.KeyUserPath}{name}.json");
    }

    public async Task<T?> GetUserFromStorage<T>(string name)
    {
        await using var openStream = File.OpenRead($"{AppSettings.KeyUserPath}{name}.json");
        return await JsonSerializer.DeserializeAsync<T>(openStream);
    }

    // public async Task CreateNewSource(AbstractSource source)
    // {
    //     if (!Directory.Exists(source.Path))
    //     {
    //         Directory.CreateDirectory(source.Path);
    //     }
    //
    //     if (source.GetType() == typeof(Note))
    //     {
    //         await using var createStream = File.Create(source.Path + source.Name);
    //         return;
    //     }
    //     Directory.CreateDirectory(source.Path + source.Name);
    // }
}