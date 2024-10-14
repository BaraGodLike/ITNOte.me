using Microsoft.Extensions.Configuration;

namespace ITNotionWPF;

public sealed class Settings
{
    private static readonly IConfigurationRoot Config = new ConfigurationBuilder()
        .AddJsonFile("appsettings.json")
        .AddEnvironmentVariables()
        .Build();


    public static readonly Settings AppSettings = Config.GetRequiredSection("Settings").Get<Settings>()!;
    
    public required string KeyLog { get; init; }
    public required string KeyUserPath { get; init; }
}
