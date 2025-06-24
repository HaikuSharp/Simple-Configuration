using SC.Abstraction;
using System;
using System.ComponentModel;

namespace SC.Extensions;

public static class ConfigurationExtensions
{
    public static bool TryGetSection(this IConfiguration configuration, string prefix, out IConfigurationSection section)
    {
        section = configuration.GetSection(prefix);
        return section is not null;
    }

    public static bool TryGetValue(this IConfiguration configuration, string fullPath, out string value)
    {
        value = configuration.GetValue(fullPath);
        return value is not null;
    }

    public static bool TryGetValue<T>(this IConfiguration configuration, string fullPath, out T value)
    {
        value = configuration.GetValue<T>(fullPath);
        return value is not null;
    }

    public static bool TryGetValue(this IConfiguration configuration, string fullPath, Type type, out object value)
    {
        value = configuration.GetValue(fullPath, type);
        return value is not null;
    }

    public static bool TryGetValue(this IConfiguration configuration, string fullPath, TypeConverter converter, out object value)
    {
        value = configuration.GetValue(fullPath, converter);
        return value is not null;
    }

    public static T GetValue<T>(this IConfiguration configuration, string fullPath) => (T)configuration.GetValue(fullPath, typeof(T));

    public static object GetValue(this IConfiguration configuration, string fullPath, Type type) => type == typeof(byte[]) ? GetBits(configuration.GetValue(fullPath)) : configuration.GetValue(fullPath, TypeDescriptor.GetConverter(GetObjectType(type)));

    public static object GetValue(this IConfiguration configuration, string fullPath, TypeConverter converter)
    {
        string value = configuration.GetValue(fullPath);
        return value is null ? default : converter.CanConvertFrom(typeof(string)) ? converter.ConvertFromInvariantString(value) : null;
    }

    private static byte[] GetBits(string base64) => base64 == string.Empty ? [] : Convert.FromBase64String(base64);

    private static Type GetObjectType(Type type) => type.IsGenericType && type.GetGenericTypeDefinition() == typeof(Nullable<>) ? Nullable.GetUnderlyingType(type) : type;
}
