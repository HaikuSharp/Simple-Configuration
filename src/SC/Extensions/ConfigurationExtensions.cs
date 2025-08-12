using SC.Abstraction;
using System.Threading.Tasks;

namespace SC.Extensions;

public static class ConfigurationExtensions
{
    public static IConfigurationSection GetSection(this IConfiguration configuration, string path) => new ConfigurationSection(configuration, path);

    public static void Save(this IConfiguration configuration) => configuration.Save(null);

    public static void Load(this IConfiguration configuration) => configuration.Load(null);

    public static async Task SaveAsync(this IConfiguration configuration) => await configuration.SaveAsync(null);

    public static async Task LoadAsync(this IConfiguration configuration) => await configuration.LoadAsync(null);
}
