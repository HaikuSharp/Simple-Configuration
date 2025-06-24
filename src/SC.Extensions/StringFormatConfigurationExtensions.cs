using SC.Abstraction;
using System.Collections.Generic;
#if NETFRAMEWORK
using System.Linq;
#endif

namespace SC.Extensions;

public static class StringFormatConfigurationExtensions
{
    public static string Format(this IConfiguration configuration, string str) => str.Format(configuration);

    public static string Format(this string str, IConfiguration configuration) => str.Format(configuration.Pairs);

    public static string Format(this string str, IEnumerable<ConfigurationPathValuePair> pairs)
    {
        if(string.IsNullOrWhiteSpace(str) || !str.Contains('{') || !str.Contains('}')) return str;
        foreach(var pair in pairs) str = str.Format(pair);
        return str;
    }

    public static string Format(this string str, ConfigurationPathValuePair pair)
    {
        pair.Destruct(out var path, out var value);
        str = str.Replace('{' + path + '}', value);
        return str;
    }
}