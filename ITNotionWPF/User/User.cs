// using ITNotion.Commands;
// using ITNotion.Notes;

using ITNotionWPF.Storage;

namespace ITNotionWPF.User;

public class User(string name, string? password)
{
    public string Name { get; } = name;
    public string? Password { get; } = password;
    // public CommandHistory CommandHistory { get; } = new();
    // public DirectoryNotes Sources { get; } = new("Notes", null);
    private static readonly Storage.Storage Storage = new(new LocalRepository());

    // public async Task AddSource(AbstractSource source)
    // {
    //     var t1 = Sources.AddSource(source);
    //     var t2 = Storage.CreateNewSource(source);
    //     await Task.WhenAll(t1, t2);
    // }
}