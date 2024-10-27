
using System.Text.Json.Serialization;
using ITNOte.me.Model.Notes;

namespace ITNOte.me.Model.User;

public class User
{
    public string Name { get; init; }
    public string? Password { get; init; }
    public Folder GeneralFolder { get; init; }
    
    public User(string name, string? password, Folder generalFolder)
    {
        Name = name;
        Password = password;
        GeneralFolder = generalFolder;
    }

    [JsonConstructor]
    public User()
    {
        
    }
    
    public User(string name, string? password) : this(name, password, new Folder(name))
    {
    }
    public override string ToString() => Name;
}
