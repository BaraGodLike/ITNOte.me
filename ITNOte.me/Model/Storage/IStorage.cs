// using ITNotion.Notes;

namespace ITNOte.me.Model.Storage;

public interface IStorage
{
    Task SaveRegistryUser<T>(T user);
    bool HasNicknameInStorage(string name);
    Task<T?> GetUserFromStorage<T>(string name);
    Task CreateNewSource(string path, string name);
    Task WriteInNote(string path, string name, string text);

}