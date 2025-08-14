using SC.Abstraction;
using System.Threading.Tasks;

namespace SC.Extensions;

public static class ConfigurationExtensions
{
    public static IConfigurationSection GetSection(this IConfiguration configuration, string path) => new ConfigurationSection(configuration, path);

    public static bool TryGetOption<T>(this IConfiguration configuration, string path, out IConfigurationOption<T> option)
    {
        option = configuration.GetOption<T>(path);
        return option is not null;
    }

    public static bool TryGetOption<T>(this IReadOnlyConfiguration configuration, string path, out IReadOnlyConfigurationOption<T> option)
    {
        option = configuration.GetOption<T>(path);
        return option is not null;
    }

    public static bool TryGetValue<T>(this IReadOnlyConfiguration configuration, string path, out T value)
    {
        if(configuration.TryGetOption<T>(path, out var option))
        {
            value = option.Value;
            return true;
        }

        value = default;
        return false;
    }

    public static T GetValue<T>(this IReadOnlyConfiguration configuration, string path) => configuration.TryGetOption<T>(path, out var option) ? option.Value : default;

    public static void SetValue<T>(this IConfiguration configuration, string path, T value)
    {
        if(!configuration.TryGetOption<T>(path, out var option)) return;
        option.Value = value;
    }

    public static void Save(this IConfiguration configuration) => configuration.Save(null);

    public static void Load(this IConfiguration configuration) => configuration.Load(null);

    public static async Task SaveAsync(this IConfiguration configuration) => await configuration.SaveAsync(null);

    public static async Task LoadAsync(this IConfiguration configuration) => await configuration.LoadAsync(null);
}
