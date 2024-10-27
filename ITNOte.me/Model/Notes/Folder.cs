using System.Collections.ObjectModel;
using System.Runtime.Serialization;
using System.Text.Json.Serialization;

namespace ITNOte.me.Model.Notes;

[Serializable]
public class Folder : AbstractSource
{
    public Folder(string name, Folder? parent = null) : base(name, parent)
    {
        Children = [];
        Type = nameof(Folder);
        _ = Storage.Storage.RepoStorage.CreateNewSource(Path, Name, false);
    }

    public Folder() : base()
    {
        
    }
}

