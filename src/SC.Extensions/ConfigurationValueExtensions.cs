using SC.Abstraction;
using System;
using System.ComponentModel;
#if NETFRAMEWORK
using System.Linq;
#endif

namespace SC.Extensions;

public static class ConfigurationValueExtensions
{
    public static bool TryGetSection(this IConfiguration configuration, ConfigurationPath prefix, out IConfigurationSection section)
    {
        section = configuration.GetSection(prefix);
        return section is not null;
    }

    public static bool TryGetValue(this IConfiguration configuration, ConfigurationPath fullPath, out ConfigurationValue value)
    {
        value = configuration.GetValue(fullPath);
        return !value.IsNull;
    }

    public static bool TryGetValue<T>(this IConfiguration configuration, ConfigurationPath fullPath, out T value)
    {
        value = configuration.GetValue<T>(fullPath);
        return value is not null;
    }

    public static bool TryGetValue(this IConfiguration configuration, ConfigurationPath fullPath, Type type, out object value)
    {
        value = configuration.GetValue(fullPath, type);
        return value is not null;
    }

    public static bool TryGetValue(this IConfiguration configuration, ConfigurationPath fullPath, TypeConverter converter, out object value)
    {
        value = configuration.GetValue(fullPath, converter);
        return value is not null;
    }

    public static T GetValue<T>(this IConfiguration configuration, ConfigurationPath fullPath) => configuration.GetValue(fullPath).To<T>();

    public static object GetValue(this IConfiguration configuration, ConfigurationPath fullPath, Type type) => configuration.GetValue(fullPath).To(type);

    public static object GetValue(this IConfiguration configuration, ConfigurationPath fullPath, TypeConverter converter) => configuration.GetValue(fullPath).To(converter);
}
