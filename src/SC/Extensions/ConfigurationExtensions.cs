using SC.Abstraction;

namespace SC.Extensions;

public static class ConfigurationExtensions
{
    public static IConfigurationSection GetSection(this IConfiguration configuration, string path) => new ConfigurationSection(configuration, path);

    public static void AddCell<T>(this IConfiguration configuration, string path) => configuration.AddCell<T>(default);
}
