// using ITNotion.Notes;

using Microsoft.VisualBasic;

namespace ITNOte.me.Model.Storage;

public class Storage(IStorage repo) : IStorage
{
    public static readonly Storage RepoStorage = new(new LocalRepository());
    
    public static string HashPassword(string password)
    {
        return Conversion.Hex(password.Select((t, i) => 
            t * (long) Math.Pow(7, i)).Sum());
    }

    public bool HasNicknameInStorage(string name)
    {
        return repo.HasNicknameInStorage(name);
    }

    public async Task SaveUser<T>(T user)
    {
        await repo.SaveUser(user);
    }

    public async Task<T?> GetUserFromStorage<T>(string name)
    {
        return await repo.GetUserFromStorage<T>(name);
    }

    public async Task CreateNewSource(string path, string name, bool isFile)
    {
        await repo.CreateNewSource(path, name, isFile);
    }

    public async Task WriteInNote(string path, string name, string text)
    {
        await repo.WriteInNote(path, name, text);
    }

    public async Task<string> ReadNote(string path, string name)
    {
        return await repo.ReadNote(path, name);
    }
    
}