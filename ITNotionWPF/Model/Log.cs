using System.IO;
using static ITNotionWPF.Settings;

namespace ITNotionWPF.Model;

public static class Log
{
    public static async Task LogInformation<T>(T user, string text)
    {
        if (!Directory.Exists(AppSettings.KeyLog))
        {
            Directory.CreateDirectory(AppSettings.KeyLog);
        }
        
        await File.AppendAllTextAsync($"{AppSettings.KeyLog}logs.log",
            $"[{DateTime.Now}] -INFORMATION- {user} {text}.\n");
    }
    
    // public static async Task LogInformation(UserDto user, AbstractCommand command)
    // {
    //     if (!Directory.Exists("Storage/Logs/"))
    //     {
    //         Directory.CreateDirectory("Storage/Logs");
    //     }
    //     
    //     await File.AppendAllTextAsync("Storage/Logs/logs.log",
    //         $"[{DateTime.Now}] -INFORMATION- {user.User?.Name} used {command.Name}.\n");
    // }
    //
    public static async Task LogWarning<T>(Exception exception, T user)
    {
        if (!Directory.Exists(AppSettings.KeyLog))
        {
            Directory.CreateDirectory(AppSettings.KeyLog);
        }
        
        await File.AppendAllTextAsync($"{AppSettings.KeyLog}logs.log",
            $"[{DateTime.Now}] -WARNING- {user} {exception}.\n");
    }
}
