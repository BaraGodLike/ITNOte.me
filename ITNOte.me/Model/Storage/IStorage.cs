// using ITNotion.Notes;

namespace ITNOte.me.Model.Storage;

public interface IStorage
{
    Task SaveUser<T>(T user);
    bool HasNicknameInStorage(string name);
    Task<T?> GetUserFromStorage<T>(string name);
    Task CreateNewSource(string path, string name, bool isFile);
    Task WriteInNote(string path, string name, string text);
    Task<string> ReadNote(string path, string name);

}