namespace ITNOte.me.Model.Notes;

public class AbstractSource(string name, Folder? parent = null) : IComparable<AbstractSource>
{
    public string Name { get; private set; } = name;
    protected Folder? Parent { get; set; } = parent;
    protected string Path { get; set; } = $"{parent?.Path}/{parent?.Name}";
    

    public int CompareTo(AbstractSource? other)
    {
        return string.Compare(Path, other?.Path, StringComparison.Ordinal);
    }
}