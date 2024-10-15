
namespace ITNOte.me.Model.User;

public class User(string name, string? password)
{
    public string Name { get; } = name;
    public string? Password { get; } = password;
    // public CommandHistory CommandHistory { get; } = new();
    // public DirectoryNotes Sources { get; } = new("Notes", null);

    // public async Task AddSource(AbstractSource source)
    // {
    //     var t1 = Sources.AddSource(source);
    //     var t2 = Storage.CreateNewSource(source);
    //     await Task.WhenAll(t1, t2);
    // }

    public override string ToString()
    {
        return Name;
    }
}