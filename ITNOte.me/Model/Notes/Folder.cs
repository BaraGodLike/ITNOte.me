using System.Collections.ObjectModel;

namespace ITNOte.me.Model.Notes;

public class Folder : AbstractSource
{
    public ObservableCollection<AbstractSource> Children { get; private set; }

    public Folder(string name, Folder? parent = null) : base(name, parent)
    {
        Children = [];
        _ = Storage.Storage.RepoStorage.CreateNewSource(Path, Name, false);
    }

}