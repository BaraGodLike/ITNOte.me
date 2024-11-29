using System.Text.Json.Serialization;

namespace ITNOte.me.Model.User;

public class UserDto
{
    public string Name { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
    public int General_Folder_Id { get; set; } = 0;
}