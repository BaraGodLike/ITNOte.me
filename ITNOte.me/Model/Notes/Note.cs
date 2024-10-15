using System.IO;

namespace ITNOte.me.Model.Notes;

public class Note(string name, Folder? parent = null) : AbstractSource(name, parent)
{
    private List<string> Backup { get; } = new(100);
    private int _curBackupIndex = 0;

    public async Task MakeBackup()
    {
        var now = await File.ReadAllTextAsync($"{Path}/{Name}");
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
    }

    public bool nowInLastBackup()
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
}