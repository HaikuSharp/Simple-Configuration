using SC.Abstraction;

namespace SC.Extensions;

public static class ConfigurationSectionExtensions
{
    public static T GetValue<T>(this IConfigurationSection section, string path) => section.GetValue<T>(new ConfigurationPath(path));

    public static void SetValue<T>(this IConfigurationSection section, string path, T value) => section.SetValue(new ConfigurationPath(path), value);
}
