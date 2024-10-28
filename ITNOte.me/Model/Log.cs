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
