namespace ITNOte.me.Model.Notes;

public class AbstractSource : IComparable<AbstractSource>
{
    public string Name { get; private set; }
    protected Folder? Parent { get; set; }
    protected string Path { get; set; }

    protected AbstractSource(string name, Folder? parent = null)
    {
        Name = name;
        Parent = parent;
        Path = $"{parent?.Path}/{parent?.Name}";
        parent?.Children.Add(this);
    }
    
    public int CompareTo(AbstractSource? other)
    {
        return string.Compare(Path, other?.Path, StringComparison.Ordinal);
    }
}