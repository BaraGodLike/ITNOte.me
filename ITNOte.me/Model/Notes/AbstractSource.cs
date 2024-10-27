using System;
using System.Collections.ObjectModel;
using System.Runtime.Serialization;
using System.Text.Json.Serialization;

namespace ITNOte.me.Model.Notes
{
    [Serializable]
    public abstract class AbstractSource : IComparable<AbstractSource>
    {
        public string Name { get; set; }
        [JsonIgnore]
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

        [JsonConstructor]
        protected AbstractSource()
        {
            
        }
        
        public int CompareTo(AbstractSource? other)
        {
            return string.Compare(Path, other?.Path, StringComparison.Ordinal);
        }
    }
}