// using ITNotion.Notes;

using Microsoft.VisualBasic;

namespace ITNotionWPF.Model.Storage;

public class Storage(IStorage repo) : IStorage
{
    public static readonly Storage RepoStorage = new Storage(new LocalRepository());
    
    public static string HashPassword(string password)
    {
        return Conversion.Hex(password.Select((t, i) => 
            t * (long) Math.Pow(7, i)).Sum());
    }

    public bool HasNicknameInStorage(string name)
    {
        return repo.HasNicknameInStorage(name);
    }

    public async Task SaveRegistryUser<T>(T user)
    {
        await repo.SaveRegistryUser(user);
    }

    public async Task<T?> GetUserFromStorage<T>(string name)
    {
        return await repo.GetUserFromStorage<T>(name);
    }

    // public async Task CreateNewSource(AbstractSource source)
    // {
    //     await repo.CreateNewSource(source);
    // }
}