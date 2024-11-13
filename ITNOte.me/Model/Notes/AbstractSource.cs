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

        public static List<string> BannedNames = ["con", "prn", "com1", "com2", "com3", "com4", "lpt1", "lpt2", "lpt3", "lpt4"];
        
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