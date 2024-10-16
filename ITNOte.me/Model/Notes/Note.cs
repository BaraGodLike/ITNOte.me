using System.IO;

namespace ITNOte.me.Model.Notes;

public class Note : AbstractSource
{
    private List<string> Backup { get; } = new(100);
    private int _curBackupIndex;

    public Note(string name, Folder? parent = null) : base(name, parent)
    {
        _ = Storage.Storage.RepoStorage.CreateNewSource(Path, Name, true);
    }
    
    public async Task MakeBackup()
    {
        var now = await GetText();
        if (!now.Equals(Backup[_curBackupIndex]))
        {
            if (Backup.Count == 100) Backup.RemoveAt(0);
            Backup.Add(now);
            _curBackupIndex = Backup.Count;
        }
    }

    public void CleanBackup()
    {
        Backup.Clear();
        _curBackupIndex = 0;
    }

    public bool NowInLastBackup()
    {
        return _curBackupIndex == Backup.Count;
    }

    public void NewBranchBackup()
    {
        Backup.RemoveRange(_curBackupIndex, Backup.Count - 1);
    }

    public async Task FromBackup()
    {
        await Storage.Storage.RepoStorage.WriteInNote(Path, Name, Backup[_curBackupIndex]);
    }

    public async Task<string> GetText()
    {
        return await Storage.Storage.RepoStorage.ReadNote(Path, Name);
    }
}