using SC.Abstraction;
using System;

namespace SC.Extensions;

public static class ConfigurationSectionExtensions
{
    public static T GetValue<T>(this IConfigurationSection section, string path) => section.GetValue<T>(new ConfigurationPath(path));

    public static void SetValue<T>(this IConfigurationSection section, string path, T value) => section.SetValue(new ConfigurationPath(path), value);

    public static T GetValueOrDefault<T>(this IConfigurationSection section, string path, T defaultValue) => section.GetValueOrDefault(new ConfigurationPath(path), defaultValue);

    public static T GetValueOrDefault<T>(this IConfigurationSection section, ConfigurationPath path, T defaultValue) => section.GetValueOrDefault(path.GetEnumerator(), defaultValue);

    public static T GetValueOrDefault<T>(this IConfigurationSection section, ConfigurationPathEnumerator enumerator, T defaultValue) => section.TryGetValue(enumerator, out T value) ? value : defaultValue;

    public static T GetValueOrDefault<T>(this IConfigurationSection section, string path, Func<T> getDefault) => section.GetValueOrDefault(new ConfigurationPath(path), getDefault);

    public static T GetValueOrDefault<T>(this IConfigurationSection section, ConfigurationPath path, Func<T> getDefault) => section.GetValueOrDefault(path.GetEnumerator(), getDefault);

    public static T GetValueOrDefault<T>(this IConfigurationSection section, ConfigurationPathEnumerator enumerator, Func<T> getDefault) => section.TryGetValue(enumerator, out T value) ? value : getDefault();
}
