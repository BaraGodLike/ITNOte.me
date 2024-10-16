using Microsoft.Extensions.Configuration;

namespace ITNOte.me.Model;

public sealed class ConfigurationSettings
{
    private static readonly IConfigurationRoot Config = new ConfigurationBuilder()
        .AddJsonFile("appsettings.json")
        .AddEnvironmentVariables()
        .Build();
    
    public static readonly ConfigurationSettings AppConfigurationSettings = Config.GetRequiredSection("Settings").
        Get<ConfigurationSettings>()!;
    
    public required string KeyLog { get; init; }
    public required string KeyUserPath { get; init; }
    public required string KeyUserSourcesPath { get; init; }
}
