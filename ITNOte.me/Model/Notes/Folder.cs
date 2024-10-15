namespace ITNOte.me.Model.Notes;

public class Folder : AbstractSource
{
    public List<AbstractSource> Children { get; private set; }

    public Folder(string name, Folder? parent = null) : base(name, parent)
    {
        Children = [];
    }

}