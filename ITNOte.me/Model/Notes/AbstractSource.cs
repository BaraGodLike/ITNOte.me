using System.Collections.ObjectModel;

namespace ITNOte.me.Model.Notes
{
    [Serializable]
    public abstract class AbstractSource : IComparable<AbstractSource>
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public Folder? Parent { get; set; }
        public string Path { get; set; }
        
        public ObservableCollection<AbstractSource>? Children { get; set; }
        public string Type { get; set; }

        protected AbstractSource(string name, Folder? parent = null)
        {
            Name = name;
            Parent = parent;
            Path = $"{parent?.Path}/{parent?.Name}";
            if (parent != null && !parent.Children!.Contains(this))
                parent.Children.Add(this);
        }

        protected AbstractSource()
        {
            
        }
        
        public int CompareTo(AbstractSource? other)
        {
            return string.Compare(Path, other?.Path, StringComparison.Ordinal);
        }
    }
}