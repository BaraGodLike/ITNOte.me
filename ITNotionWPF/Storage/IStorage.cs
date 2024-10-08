// using ITNotion.Notes;

using ITNotionWPF.User;

namespace ITNotionWPF.Storage;

public interface IStorage
{
    Task SaveRegistryUser(UserDto user);
    bool HasNicknameInStorage(string name);
    Task<UserDto?> GetUserFromStorage(string name);
    // Task CreateNewSource(AbstractSource source);

}