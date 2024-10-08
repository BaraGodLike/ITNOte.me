// using ITNotion.Notes;

using ITNotionWPF.User;
using Microsoft.VisualBasic;

namespace ITNotionWPF.Storage;

public class Storage(IStorage repo) : IStorage
{
    public static string HashPassword(string password)
    {
        return Conversion.Hex(password.Select((t, i) => 
            t * (long) Math.Pow(7, i)).Sum());
    }

    public bool HasNicknameInStorage(string name)
    {
        return repo.HasNicknameInStorage(name);
    }

    public async Task SaveRegistryUser(UserDto user)
    {
        await repo.SaveRegistryUser(user);
    }

    public async Task<UserDto?> GetUserFromStorage(string name)
    {
        return await repo.GetUserFromStorage(name);
    }

    public async Task<string?> GetPasswordUser(string name)
    {
        return (await GetUserFromStorage(name))?.User?.Password;
    }

    // public async Task CreateNewSource(AbstractSource source)
    // {
    //     await repo.CreateNewSource(source);
    // }
}