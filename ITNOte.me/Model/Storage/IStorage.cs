using System.Collections.ObjectModel;
using ITNOte.me.Model.Notes;

namespace ITNOte.me.Model.Storage;

public interface IStorage
{
    
    Task SaveUser<T>(T user);
    Task<bool> HasNicknameInStorage(string name);
    Task<T?> GetUserFromStorage<T>(string name);
    Task CreateNewSource(AbstractSource source);
    Task WriteInNote(int id, string name, string text);
    Task<string> ReadNote(int id, string name);
    Task RenameNote(int id, string newName);
    Task DeleteNote(int id);
    Task<ObservableCollection<AbstractSource>> GetAllChildren(int id);
}