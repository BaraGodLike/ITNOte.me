namespace ITNOte.me.Model.Notes;

[Serializable]
public class Folder : AbstractSource
{
    public Folder(string name, Folder? parent = null) : base(name, parent)
    {
        Children = [];
        Type = nameof(Folder);
    }
    
}

