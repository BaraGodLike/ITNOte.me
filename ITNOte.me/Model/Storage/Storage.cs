using ITNOte.me.Model.Notes;

namespace ITNOte.me.Model.Storage;

public class Storage(IStorage repo) : IStorage
{
    public static Storage RepoStorage = new(new Database());
    public static readonly PasswordHasher Hasher = new PasswordHasher();
    
    
    public async Task<bool> HasNicknameInStorage(string name)
    {
        return await repo.HasNicknameInStorage(name);
    }

    public async Task SaveUser<T>(T user)
    {
        await repo.SaveUser(user);
    }

    public async Task<T?> GetUserFromStorage<T>(string name)
    {
        return await repo.GetUserFromStorage<T>(name);
    }

    public async Task CreateNewSource(AbstractSource source)
    {
        await repo.CreateNewSource(source);
    }

    public async Task WriteInNote(int id, string name, string text)
    {
        await repo.WriteInNote(id, name, text);
    }

    public async Task<string> ReadNote(int id, string name)
    {
        return await repo.ReadNote(id, name);
    }
    
}