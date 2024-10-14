// using ITNotion.Notes;

namespace ITNotionWPF.Model.Storage;

public interface IStorage
{
    Task SaveRegistryUser<T>(T user);
    bool HasNicknameInStorage(string name);
    Task<T?> GetUserFromStorage<T>(string name);
    // Task CreateNewSource(AbstractSource source);

}