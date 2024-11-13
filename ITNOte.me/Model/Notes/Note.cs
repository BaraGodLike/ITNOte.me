
namespace ITNOte.me.Model.Notes;

[Serializable]
public class Note : AbstractSource
{
    public string Content = "";
    private List<string> Backup { get; } = new(100);
    private int _curBackupIndex;

    public Note(string name, Folder? parent = null) : base(name, parent)
    {
        Type = nameof(Note);
        _ = Storage.Storage.RepoStorage.CreateNewSource(this);
    }

    public Note() : base()
    {
        
    }
    
    

    public async Task MakeBackup()
    {
        if (!Content.Equals(Backup[_curBackupIndex]))
        {
            if (Backup.Count == 100) Backup.RemoveAt(0);
            Backup.Add(Content);
            _curBackupIndex = Backup.Count - 1;
        }
    }

    public void CleanBackup()
    {
        Backup.Clear();
        _curBackupIndex = -1;
    }

    public bool NowInLastBackup()
    {
        return _curBackupIndex + 1 == Backup.Count;
    }

    public void NewBranchBackup()
    {
        Backup.RemoveRange(_curBackupIndex, Backup.Count - 1);
    }
    
    public async Task<string> GetTextFromFile()
    {
        return await Storage.Storage.RepoStorage.ReadNote(Id, Name);
    }

    public async Task Save()
    {
        await Storage.Storage.RepoStorage.WriteInNote(Id, Name, Content);
    }
}