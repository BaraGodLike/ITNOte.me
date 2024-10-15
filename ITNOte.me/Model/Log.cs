using System.IO;
using static ITNOte.me.Model.ConfigurationSettings;

namespace ITNOte.me.Model;

public static class Log
{
    public static async Task LogInformation<T>(T user, string text)
    {
        if (!Directory.Exists(AppConfigurationSettings.KeyLog))
        {
            Directory.CreateDirectory(AppConfigurationSettings.KeyLog);
        }
        
        await File.AppendAllTextAsync($"{AppConfigurationSettings.KeyLog}logs.log",
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
        if (!Directory.Exists(AppConfigurationSettings.KeyLog))
        {
            Directory.CreateDirectory(AppConfigurationSettings.KeyLog);
        }
        
        await File.AppendAllTextAsync($"{AppConfigurationSettings.KeyLog}logs.log",
            $"[{DateTime.Now}] -WARNING- {user} {exception}.\n");
    }
}
