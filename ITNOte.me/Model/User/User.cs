
using ITNOte.me.Model.Notes;

namespace ITNOte.me.Model.User;

public class User(string name, string? password)
{
    public string Name { get; } = name;
    public string? Password { get; } = password;
    public Folder GeneralFolder { get; } = new(name);

    public override string ToString() => Name;
}