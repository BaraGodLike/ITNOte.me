using System.Collections.ObjectModel;
using System.Data;
using Dapper;
using ITNOte.me.Model.Notes;
using Npgsql;

namespace ITNOte.me.Model.Storage;

public class Database : IStorage
{
    private readonly string _connectionString = ConfigurationSettings.AppConfigurationSettings.ConnectionString;
    
    private async Task<IDbConnection> GetConnection()
    {
        var connection = new NpgsqlConnection(_connectionString);
        await connection.OpenAsync();
        return connection;
    }
    
    public async Task SaveUser<T>(T user)
    {
        if (user is User.User userData && !await HasNicknameInStorage(userData.Name))
        {
            using var connection = await GetConnection();
            const string query = $"""INSERT INTO USERS (NAME, PASSWORD) VALUES (@UserName, @UserPassword);""";
            await connection.ExecuteAsync(query, new
            {
                UserName = userData.Name,
                UserPassword = userData.Password
            });
            const string queryFolder = $"""INSERT INTO FOLDERS (NAME) VALUES (@UserName) RETURNING ID;""";

            var folderId = await connection.ExecuteScalarAsync<int>(queryFolder, new
            {
                UserName = userData.Name
            });
            const string querySetFolder = $"""UPDATE USERS SET GENERAL_FOLDER_ID = @FolderId WHERE NAME = @Name;""";
            await connection.ExecuteAsync(querySetFolder, new
            {
                FolderId = folderId,
                Name = userData.Name
            });
            userData.GeneralFolder.Id = folderId;
            userData.General_Folder_Id = folderId;
        }
    }

    public async Task<bool> HasNicknameInStorage(string name)
    {
        var connection = await GetConnection();
        const string query = $"""SELECT EXISTS(SELECT 1 FROM USERS WHERE NAME = @KeyValue);""";
        return (bool)await connection.ExecuteScalarAsync(query, new { KeyValue = name });
    }

    public async Task<T?> GetUserFromStorage<T>(string name)
    {
        var connection = await GetConnection();
        const string query = $"""SELECT NAME, PASSWORD, GENERAL_FOLDER_ID FROM USERS WHERE NAME = @Name""";
        var result = await connection.QuerySingleOrDefaultAsync<User.User>(query, new { Name = name });
        if (result == null) return default;
        var generalFolder = new Folder(result.Name)
        {
            Children = await GetAllChildren(result.General_Folder_Id),
            Id = result.General_Folder_Id
        };
        return (T)(object)new User.User { Name = result.Name, Password = result.Password, 
            GeneralFolder = generalFolder, General_Folder_Id = result.General_Folder_Id};
    }

    public async Task CreateNewSource(AbstractSource source)
    {
        var connection = await GetConnection();
        const string query = $"""INSERT INTO NOTES (PARENT_ID, NAME, CONTENT) VALUES (@ParentId, @Name, @Content) RETURNING ID""";
        var idNote = await connection.ExecuteScalarAsync(query, new
        {
            ParentId = source.Parent!.Id,
            Name = source.Name,
            Content = string.Empty
        });
        source.Id = (int)idNote;
    }

    public async Task WriteInNote(int id, string name, string text)
    {
        var connection = await GetConnection();
        const string query = $"""UPDATE NOTES SET CONTENT = @Text WHERE ID = @Id AND NAME = @Name""";
        await connection.ExecuteAsync(query, new
        {
            Text = text,
            Id = id,
            Name = name
        });
    }

    public async Task<string> ReadNote(int id, string name)
    {
        var connection = await GetConnection();
        const string query = $"""SELECT CONTENT FROM NOTES WHERE ID = @Id""";
        var content = await connection.QuerySingleOrDefaultAsync<string>(query, new { Id = id});
        return content ?? string.Empty;
    }

    public async Task RenameNote(int id, string newName)
    {
       var connection = await GetConnection();
       const string query = $"""UPDATE NOTES SET NAME = @NewName WHERE ID = @Id""";
       await connection.ExecuteAsync(query, new
       {
           NewName = newName,
           Id = id
       });
    }

    public async Task DeleteNote(int id)
    {
        var connection = await GetConnection();
        const string query = $"""DELETE FROM NOTES WHERE ID = @Id""";
        await connection.ExecuteAsync(query, new
        {
            Id = id
        });
    }

    public async Task<ObservableCollection<AbstractSource>> GetAllChildren(int rootId)
    {
        using var connection = await GetConnection();
        const string query = $"""SELECT * FROM NOTES WHERE PARENT_ID = (@RootId)""";
        var notes = await connection.QueryAsync<Note>(query, new { RootId = rootId });
        return new ObservableCollection<AbstractSource>(notes);
    }
}