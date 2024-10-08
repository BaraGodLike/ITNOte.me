using System.IO;
using System.Text.Json;
using ITNotionWPF.User;
// using ITNotion.Notes;

namespace ITNotionWPF.Storage;

public class LocalRepository : IStorage
{
    public async Task SaveRegistryUser(UserDto user)
    {
        if (!Directory.Exists("Storage/Users/"))
        {
            Directory.CreateDirectory("Storage/Users");
        }
        
        await using var createStream = File.Create($"Storage/Users/{user.User!.Name}.json");
        await JsonSerializer.SerializeAsync(createStream, user);
    }
    
    
    public bool HasNicknameInStorage(string name)
    {
        return File.Exists($"Storage/Users/{name}.json");
    }

    public async Task<UserDto?> GetUserFromStorage(string name)
    {
        await using var openStream = File.OpenRead($"Storage/Users/{name}.json");
        return await JsonSerializer.DeserializeAsync<UserDto>(openStream);
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